using BackendPsychSite.API.DTOs;
using BackendPsychSite.UseCases.Interfaces;
using BackendPsychSite.UseCases.Utils;
using CsvHelper;
using CsvHelper.Configuration;
using MathNet.Numerics.Statistics;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using System.Globalization;
namespace BackendPsychSite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataSetController : ControllerBase
    {
        private IDataSetService _dsService;
        private IMinioClient _minioClient;
        public DataSetController(IDataSetService service, IMinioClient minioClient)
        {
            _dsService = service;
            _minioClient = minioClient;
        }
        [HttpGet("preprocess")] //TODO: REFACTOR!
        public async Task<IActionResult> PreprocessDataSet([FromQuery] string userPart, [FromQuery] string projectPart, [FromQuery] string dataSetName, [FromQuery] bool doMedian = true)
        {
            try
            {
                // Проверяем, существует ли бакет
                var bucketExistsArgs = new BucketExistsArgs().WithBucket(userPart);
                if (!await _minioClient.BucketExistsAsync(bucketExistsArgs))
                {
                    return NotFound($"Bucket '{userPart}' does not exist.");
                }

                List<double?[]> values = new List<double?[]>();
                using (var memoryStream = new MemoryStream())
                {
                    var getObjectArgs = new GetObjectArgs()
                        .WithBucket(userPart)
                        .WithObject(projectPart.ToLower().Trim() + '/' + dataSetName.Trim())
                        .WithCallbackStream(async (stream) =>
                        {
                            await stream.CopyToAsync(memoryStream);
                        });
                    await _minioClient.GetObjectAsync(getObjectArgs);
                    memoryStream.Position = 0;

                    
                    using (var reader = new StreamReader(memoryStream))
                    using (var csvReader = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        string value;
                        while (csvReader.Read())
                        {
                            for (int i = 0; csvReader.TryGetField<string>(i, out value); i++)
                            {
                                values.Add(value.Split(';').Select(s => string.IsNullOrWhiteSpace(s) ? (double?)null : double.Parse(s)).ToArray());

                            }
                        }
                    }
                }
                foreach (var value in values)
                {
                    var validData = value.Where(x => x.HasValue).Select(x => x.Value).ToList();
                    double median = Statistics.Median(validData);
                    double mean = Statistics.Mean(validData);
                    double q1 = Statistics.LowerQuartile(validData);
                    double q3 = Statistics.UpperQuartile(validData);
                    double iqr = q3 - q1;
                    double lowerBound = q1 - 1.5 * iqr;
                    double upperBound = q3 + 1.5 * iqr;
                    for (int i = 0; i < value.Length; i++)
                    {
                        if (!value[i].HasValue)
                        {
                            if (doMedian)
                            {
                                value[i] = median;
                            }
                            else
                            {
                                value[i] = mean;
                            }
                        }
                        else if (value[i] < lowerBound || value[i] > upperBound)
                        {
                            if(doMedian)
                            {
                                value[i] = median;
                            }
                            else
                            {
                                value[i] = mean;
                            }
                        }
                    }
                }
                using (var memoryStream = new MemoryStream())
                {
                    using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
                    using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        foreach (var value in values)
                        {
                            csv.WriteField(value);
                            csv.NextRecord();
                        }
                    }

                    memoryStream.Position = 0;
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(userPart)
                        .WithObject(projectPart.ToLower().Trim() + "/processed/" + dataSetName.Trim())
                        .WithStreamData(memoryStream)
                        .WithObjectSize(memoryStream.Length)
                        .WithContentType("text/csv");

                    await _minioClient.PutObjectAsync(putObjectArgs);
                }

                return Ok("Data processed and uploaded successfully.");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
        [HttpGet("download")] //TODO: Refactor later
        public async Task<IActionResult> GetCsvFileAsync([FromQuery] string userPart, [FromQuery] string projectPart, [FromQuery] string dataSetName)
        {
            try
            {
                // Проверяем, существует ли бакет
                var bucketExistsArgs = new BucketExistsArgs().WithBucket(userPart);
                if (!await _minioClient.BucketExistsAsync(bucketExistsArgs))
                {
                    return NotFound($"Bucket '{userPart}' does not exist.");
                }

                // Загружаем файл из MinIO
                var memoryStream = new MemoryStream();
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(userPart)
                    .WithObject(projectPart.ToLower().Trim() + '/' + dataSetName.Trim())
                    .WithCallbackStream(async (stream) =>
                    {
                        await stream.CopyToAsync(memoryStream);
                    });
                await _minioClient.GetObjectAsync(getObjectArgs);
                memoryStream.Position = 0;
                return File(memoryStream, "text/csv", $"{dataSetName}.csv");
            }
            catch (Exception ex)
            {
                // TODO to all catch part: Find out logger and learn it!
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] DataSetDto dataSetDto, IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            try
            {
                using (var stream = formFile.OpenReadStream())
                {
                    DataSetArgs args = new DataSetArgs { Name = dataSetDto.Name, ProjectName = dataSetDto.ProjectName, UserEmail = dataSetDto.UserEmail, Path = "None" };
                    await _dsService.CreateAsync(stream, args);
                }
                return Ok("File uploaded successfully.");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message); // TODO to all catch part: Find out logger and learn it!
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
