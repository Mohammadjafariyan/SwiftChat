using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Newtonsoft.Json;

namespace Engine.Localization
{
     public class Localizer
    {
            private static Localizer instance;
            private static readonly object lockObject = new object();

            private Dictionary<string, List<TextTranslation>> Locales = new Dictionary<string, List<TextTranslation>>();


            // Private constructor to prevent external instantiation
            private Localizer()
            {
            }

            // Public method to get the instance of the Localizer
            public static Localizer Instance
            {
                get
                {
                    // Double-check locking for thread safety
                    if (instance == null)
                    {
                        lock (lockObject)
                        {
                            if (instance == null)
                            {
                                instance = new Localizer();
                            }
                        }
                    }
                    return instance;
                }
            }

            // Your other class members go here
            public string GetText( string defaultValue,string selectedlocale=Languages.Persian)
            {

                List<TextTranslation> translations = new List<TextTranslation>();
                if (Locales.ContainsKey(selectedlocale))
                {
                    translations = Locales[selectedlocale];
                }
                else
                {
                    Locales.Add(selectedlocale, translations);
                }
                
                string filePath = HostingEnvironment.MapPath($"~/Localization/languages/default_{selectedlocale}.json"); // Replace with your JSON file path
                if (translations == null || translations?.Any()==false )
                {


                    if (!File.Exists(filePath))
                    {
                        Debug.Assert(filePath != null, nameof(filePath) + " != null");
                        File.Create(filePath);
                    }

                    var readAllText = File.ReadAllText(filePath);
                    translations = JsonConvert.DeserializeObject<List<TextTranslation>>(readAllText);


                    if (translations==null)
                    {
                        translations = new List<TextTranslation>();
                    }
                }


                var textTranslation = translations?.Find(f=>f.Name==defaultValue);

                if (textTranslation==null)
                {
                    textTranslation = new TextTranslation
                    {
                        Value = defaultValue,
                        Name = defaultValue,
                    };
                    
                    
                    if (translations==null)
                    {
                        translations = new List<TextTranslation>();
                    }
                    translations.Add(textTranslation);
                    
                    var text=JsonConvert.SerializeObject(translations);
                    
                    if (!File.Exists(filePath))
                    {
                        Debug.Assert(filePath != null, nameof(filePath) + " != null");
                        File.Create(filePath).Close();
                    }

                    Debug.Assert(filePath != null, nameof(filePath) + " != null");
                    File.WriteAllText(filePath,text);
                }


                return textTranslation?.Value ?? defaultValue;
            }

    }
}