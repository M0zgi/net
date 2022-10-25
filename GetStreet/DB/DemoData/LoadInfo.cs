using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB.Entities;

namespace DB.DemoData
{
    public class LoadInfo
    {
        public ZipCode zip { get; set; }
        //public ZipCode zip2 { get; set; }
        //public ZipCode zip3 { get; set; }
        //public ZipCode zip4 { get; set; }
        public ZipCode LoadZip1()
        {
            zip = new ZipCode()
            {
                Zip = "54001"
            };
            return zip;
        }

        public ZipCode LoadZip2()
        {
            zip = new ZipCode()
            {
                Zip = "54002"
            };
            return zip;
        }

        public ZipCode LoadZip3()
        {
            zip = new ZipCode()
            {
                Zip = "54003"
            };

            return zip;
        }

        public ZipCode LoadZip4()
        {
            zip = new ZipCode()
            {
                Zip = "54001"
            };

            return zip;
        }

        public Street LoadStreet1()
        {
            Street street = new Street()
            {
                Name = "ул. Карпенко",
                ZipCode = zip
            };

            return street;
        }

        public Street LoadStreet2()
        {
            Street street = new Street()
            {
                Name = "ул. Бутомы",
                ZipCode = zip
            };

            return street;
        }

        public Street LoadStreet3()
        {
            Street street = new Street()
            {
                Name = "ул. НИХ",
                ZipCode = zip
            };

            return street;
        }

        public Street LoadStreet4()
        {
            Street street = new Street()
            {
                Name = "ул. Крылова",
                ZipCode = zip
            };

            return street;
        }

        public Street LoadStreet5()
        {
            Street street = new Street()
            {
                Name = "ул. Садовая",
                ZipCode = zip
            };

            return street;
        }

        public Street LoadStreet6()
        {
            Street street = new Street()
            {
                Name = "ул. Чигрина",
                ZipCode = zip
            };

            return street;
        }

        public Street LoadStreet7()
        {
            Street street = new Street()
            {
                Name = "ул. Чкалова",
                ZipCode = zip
            };

            return street;
        }
    }
}
