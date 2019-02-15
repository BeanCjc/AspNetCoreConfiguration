using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var source = new Dictionary<string, string>
            {
                ["format:dateformatoptions:dateformat"] = "yyyy-MM-dd",
                ["format:dateformatoptions:timeformat"] = "HH:mm:ss",
                ["format:currencydecimalformatoptions:digits"] = "66",
                ["format:currencydecimalformatoptions:symbol"] = "￥",
                ["point"] = "(12,25)",
                ["name"] = "Bean",
                ["g"] = "Male",
                ["c:emailaddress"] = "myEmail@163.com",
                ["c:phoneno"] = "18350085448",

                ["profiles:name"] = "Bean0",
                ["profiles:g"] = "Male",
                ["profiles:c:emailaddress"] = "myEmail@1630.com",
                ["profiles:c:phoneno"] = "183500854480",

                ["zar:name"] = "Bean1",
                ["zar:g"] = "Male",
                ["zar:c:emailaddress"] = "myEmail1@163.com",
                ["zar:c:phoneno"] = "183500854481",

                ["bar:name"] = "Bean2",
                ["bar:g"] = "Male",
                ["bar:c:emailaddress"] = "myEmail2@163.com",
                ["bar:c:phoneno"] = "183500854482",


                ["profilest:zar:name"] = "Bean11",
                ["profilest:zar:g"] = "Male",
                ["profilest:zar:c:emailaddress"] = "myEmail11@163.com",
                ["profilest:zar:c:phoneno"] = "1835008544811",

                ["profilest:bar:name"] = "Bean22",
                ["profilest:bar:g"] = "Male",
                ["profilest:bar:c:emailaddress"] = "myEmail22@163.com",
                ["profilest:bar:c:phoneno"] = "1835008544822",

                ["mytestclass:name"]="BeanC",
                ["mytestclass:roles:rolename"]="rolename1",
                ["mytestclass:roles:rolename"]="rolename2"
            };
            var config = new ConfigurationBuilder().Add(new MemoryConfigurationSource { InitialData = source }).Build();



            var conf = config.GetSection("format:currencydecimalformatoptions");
            var digits = conf.GetValue("digits", 123);
            var symbol = conf.GetValue<string>("symbol");
            var point = config.GetValue("point", new Point { X = 1, Y = 1 });

            var profile = config.Get<Profile>();
            var profile1 = config.GetSection("profiles").Get(typeof(Profile));
            var profiles = config.Get<List<Profile>>();
            var profiles1 = config.Get<Dictionary<string, Profile>>();
            var profilest = config.GetSection("profilest").Get<List<Profile>>();
            var profilest1 = config.GetSection("profilest").Get<Dictionary<string, Profile>>();
            var mytestclass = config.GetSection("mytestclass").Get<MyTestClass>();
            Debug.Assert(conf.GetValue("digits", 123) == 6, "消息", "详细消息", digits);

            config.Get<Profile>().Name="123";








            var dateformat = new DateFormationOptions(config.GetSection("format:dateformatoptions"));
            var currencydecimalformat = new CurrencyDecimalFormatOptions(config.GetSection("format:currencydecimalformatoptions"));
            var format = new FormatOptions(config);
            var format1 = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("testappsetting.json").Build().GetSection("format").Get<FormatOptions>();
            Console.WriteLine(config["format:dateformatoptions:dateformat"]);
            Console.WriteLine(config["format:dateformatoptions:timeformat"]);
            Console.WriteLine(config["format:currencydecimalformatoptions:digits"]);
            Console.WriteLine(config["format:currencydecimalformatoptions:symbol"]);
            Console.WriteLine(dateformat.DateFormat);
            Console.WriteLine(dateformat.TimeFormat);
            Console.WriteLine(currencydecimalformat.Digits);
            Console.WriteLine(currencydecimalformat.Symbol);
            Console.WriteLine(format.CurrencyDecimalFormatOptions.Digits);
            Console.WriteLine(format.CurrencyDecimalFormatOptions.Symbol);
            Console.WriteLine(format.DateFormatOptions.DateFormat);
            Console.WriteLine(format.DateFormatOptions.TimeFormat);
            Console.WriteLine(format1.CurrencyDecimalFormatOptions.Digits);
            Console.WriteLine(format1.CurrencyDecimalFormatOptions.Symbol);
            Console.WriteLine(format1.DateFormatOptions.DateFormat);
            Console.WriteLine(format1.DateFormatOptions.TimeFormat);

            Console.ReadKey();

        }
    }
    class DateFormationOptions
    {
        public string DateFormat { get; set; }

        public string TimeFormat { get; set; }
        public DateFormationOptions(IConfiguration config)
        {
            DateFormat = config["dateformat"];
            TimeFormat = config["timeformat"];
        }
        public DateFormationOptions() { }
    }
    class CurrencyDecimalFormatOptions
    {
        public int Digits { get; set; }
        public string Symbol { get; set; }
        public CurrencyDecimalFormatOptions(IConfiguration config)
        {
            Digits = int.Parse(config["digits"]);
            Symbol = config["symbol"];
        }
        public CurrencyDecimalFormatOptions() { }
    }
    class FormatOptions
    {
        public DateFormationOptions DateFormatOptions { get; set; }
        public CurrencyDecimalFormatOptions CurrencyDecimalFormatOptions { get; set; }
        public FormatOptions(IConfiguration config)
        {
            DateFormatOptions = new DateFormationOptions(config.GetSection("format:dateformatoptions"));
            CurrencyDecimalFormatOptions = new CurrencyDecimalFormatOptions(config.GetSection("format:currencydecimalformatoptions"));
        }
        public FormatOptions() { }
    }
    [TypeConverter(typeof(PointTypeConverter))]
    class Point
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
    }
    class PointTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var split = value.ToString().Split(',');
            var x = Convert.ToDecimal(split[0].Trim().TrimStart('('));
            var y = Convert.ToDecimal(split[1].Trim().TrimEnd(')'));
            return new Point { X = x, Y = y };
        }
    }
    class Profile
    {
        public string Name { get; set; }
        public Gender G { get; set; }
        public ContactInfo C { get; set; }
    }
    class ContactInfo
    {
        public string EmailAddress { get; set; }
        public string PhoneNo { get; set; }
    }
    enum Gender
    {
        Male,
        Female
    }
    class ProfileLists
    {
        public List<Profile> Profilest { get; set; }
    }
    class MyTestClass
    {
        public string Name { get; set; }
        public List<Role> Roles { get; set; }
    }
    class Role
    {
        public string RoleName { get; set; }
    }
}
