using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendPsychSite.DataAccess.Entities
{
    public class DataSetBucketEntity
    {
        public string BucketName { get; set; }
        //Mail of user
        public string UserPart { get; set; }
        //Project name
        public string ProjectPart { get; set; }
        public DataSetBucketEntity(string bucketName, string userPart, string projectPart)
        {
            BucketName = bucketName;
            UserPart = userPart;
            ProjectPart = projectPart;
        }
    }
}
