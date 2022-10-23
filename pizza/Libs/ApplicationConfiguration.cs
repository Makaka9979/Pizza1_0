using System;
using System.Text.Json;

namespace Libs
{
    internal class ApplicationConfiguration
    {
        public string FName { get; private set; } = "Пiцца Харкiв";
        public static string Phone { get; private set; } = "+111111111111";
        public static string Email { get; private set; } = "info@ct-college.net";
        public string vCardTg { get; private set; } = $"BEGIN:VCARD\n VERSION:3.0\nN:Харків;Пiцца\nORG:Пiццерiя\n" +
            $"TEL;TYPE=voice,work,pref:{Phone}\nEMAIL:{Email}\nEND:VCARD";
        public double GpsLatitude { get; private set; } = 49.999366f;
        public double GpsLongitude { get; private set; } = 36.243200f;
        public string GpsTitle { get; private set; } = "ВСП «ХКТФК НТУ «ХПI»";
        public string GpsAddress { get; private set; } = "вулиця Манiзера, 4, Харкiв, Харкiвська область, Украина, 61000";

        public void Metod()
        {
            using FileStream fs = File.Open("applicationConfiguration.json", FileMode.CreateNew);
            JsonSerializer.Serialize<ApplicationConfiguration>(fs, this);
        }
    }
}
