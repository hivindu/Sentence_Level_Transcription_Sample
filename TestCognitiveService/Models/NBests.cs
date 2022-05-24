using System.Collections.Generic;

namespace TestCognitiveService.Models
{
    public class NBests
    {
        public double Confidence { get; set; }

        public string Lexical { get; set; }

        public string ITN { get; set; }

        public string MaskedITN { get; set; }

        public string Display { get; set; }

        public List<WordDetails> Words { get; set; }
    }
}
