using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyAnimeList.Data.Config
{
    public class AzureBlobSettings
    {
        public string ConnectionString { get; set; }
        public string Container { get; set; }
    }
}
