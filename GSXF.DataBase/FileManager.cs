using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GSXF.Model;
using GSXF.Auxiliary;

namespace GSXF.DataBase
{
    public class FileManager : BaseManager<UploadFile>
    {
        public override Response Delete(int ID)
        {

            Response resp = new Response();
            var file = Find(ID);



            if (file != null)
            {
                string path = file.Path;
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

            }
            return Delete(ID);
        }
    }
}
