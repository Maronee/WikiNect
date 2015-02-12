/*****************************************************************************
Project: WikiNect Kinetische Wikis
Organisation: Goethe Universität Frankfurt am Main, Institut für Informatik
Group: Wrapper Gruppe
File: ClassesForJson.cs

*****************************************************************************/
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System;

namespace Wikinect.Wrapper
{
    /// <summary>
    /// Hilsklasse, um die Antwortelemente von MediaWiki APIs als Json-Response Objekte zu übersetzen.
    /// </summary>
    public class Allcategory
    {
        /// <summary>
        /// Title - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("*")]
        public string Title { get; set; }

        /// <summary>
        /// Size - Json-Response Mitglieder
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Pages - Json-Response Mitglieder
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// Files - Json-Response Mitglieder
        /// </summary>
        public int Files { get; set; }

        /// <summary>
        /// Subcats - Json-Response Mitglieder
        /// </summary>
        public int Subcats { get; set; }

        /// <summary>
        /// Hidden - Json-Response Mitglieder
        /// </summary>
        public string Hidden { get; set; }
    }

    /// <summary>
    /// CategoryMember - Json-Response Mitglieder
    /// </summary>
    public class CategoryMember
    {
        /// <summary>
        /// PageId - Json-Response Mitglieder
        /// </summary>
        public int PageId { get; set; }

        /// <summary>
        /// Title - Json-Response Mitglieder
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Type - Json-Response Mitglieder
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Ns - Json-Response Mitglieder
        /// </summary>
        public int Ns { get; set; }

        /// <summary>
        /// Type - Json-Response Mitglieder
        /// </summary>
        public Allcategory[] Categories { get; set; }

        /// <summary>
        /// Type - Json-Response Mitglieder
        /// </summary>
        public ImageMember[] Images { get; set; }


        /// <summary>
        /// Edittoken - Json-Response Mitglieder
        /// </summary>
        public string Edittoken { get; set; }

        /// <summary>
        /// Sortkeyprefix - Json-Response Mitglieder
        /// </summary>
        public string Sortkeyprefix { get; set; }

        /// <summary>
        /// Timestamp - Json-Response Mitglieder
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// ImageMember - Json-Response Mitglieder
    /// </summary>
    public class ImageMember
    {
        /// <summary>
        /// Name - Json-Response Mitglieder
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Timestamp - Json-Response Mitglieder
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Url - Json-Response Mitglieder
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Ns - Json-Response Mitglieder
        /// </summary>
        public int Ns { get; set; }

        /// <summary>
        /// Title - Json-Response Mitglieder
        /// </summary>
        public string Title { get; set; }
    }

    /// <summary>
    /// LinkMember - Json-Response Mitglieder
    /// </summary>
    public class LinkMember
    {
        /// <summary>
        /// Fromid - Json-Response Mitglieder
        /// </summary>
        public int Fromid { get; set; }

        /// <summary>
        /// Ns - Json-Response Mitglieder
        /// </summary>
        public int Ns { get; set; }

        /// <summary>
        /// Title - Json-Response Mitglieder
        /// </summary>
        public string Title { get; set; }
    }

    /// <summary>
    /// LinkMemberContinue - Json-Response Mitglieder
    /// </summary>
    public class LinkMemberContinue
    {
        /// <summary>
        /// Alcontinue - Json-Response Mitglieder
        /// </summary>
        public string Alcontinue { get; set; }
    }

    /// <summary>
    /// PagesDeepMember - Json-Response Mitglieder
    /// </summary>
    [Serializable]
    public class PagesDeepMember
    {
        [JsonProperty("1")]
        public PagesDeepMember Page { get; set; }
        /// <summary>
        /// Ns - Json-Response Mitglieder
        /// </summary>
        public int Ns { get; set; }

        /// <summary>
        /// Title - Json-Response Mitglieder
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Missing - Json-Response Mitglieder
        /// </summary>
        public string Missing { get; set; }

        /// <summary>
        /// Starttimestamp - Json-Response Mitglieder
        /// </summary>
        public string Starttimestamp { get; set; }

        /// <summary>
        /// Edittoken - Json-Response Mitglieder
        /// </summary>
        public string Edittoken { get; set; }

        /// <summary>
        /// Deletetoken - Json-Response Mitglieder
        /// </summary>
        public string Deletetoken { get; set; }
    }



    /// <summary>
    /// Query - Json-Response Mitglieder
    /// </summary>
    public class Query
    {
        /// <summary>
        /// Categories - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("allcategories")]
        public Allcategory[] Categories { get; set; }

        /// <summary>
        /// Pages - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("categorymembers")]
        public CategoryMember[] Pages { get; set; }
        /// <summary>
        /// Images - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("allimages")]
        public ImageMember[] Images { get; set; }

        /// <summary>
        /// Links - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("alllinks")]
        public LinkMember[] Links { get; set; }

        /// <summary>
        /// Links - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("allusers")]
        public AllUsers[] Users { get; set; }
        
        /// <summary>
        /// Pages - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("pages")]
        public PagesDeepMember InfoPage { get; set; }
    }

    /// <summary>
    /// Login - Json-Response Klasse
    /// </summary>
    public class Login
    {
        /// <summary>
        /// Result - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }

        /// <summary>
        /// Token - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("token")]
        public string token { get; set; }


        /// <summary>
        /// Token - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("lgtoken")]
        public string lgtoken { get; set; }

        /// <summary>
        /// Cookieprefix - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("lguserid")]
        public int userid { get; set; }

        /// <summary>
        /// Cookieprefix - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("cookieprefix")]
        public string Cookieprefix { get; set; }

        /// <summary>
        /// Sessionid - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("sessionid")]
        public string Sessionid { get; set; }
    }

    /// <summary>
    /// Login - Json-Response Klasse
    /// </summary>
    public class Createaccount
    {
        /// <summary>
        /// Result - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("result")]
        public string Result { get; set; }

        /// <summary>
        /// Token - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("token")]
        public string token { get; set; }


        /// <summary>
        /// Token - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("userid")]
        public string userid { get; set; }

        /// <summary>
        /// Cookieprefix - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("username")]
        public int username { get; set; }
    }

    /// <summary>
    /// QueryContinue - Json-Response Mitglieder
    /// </summary>
    public class QueryContinue
    {
        //[JsonProperty("allcategories")]
        //public Allcategory[] Categories { get; set; }

        //[JsonProperty("categorymembers")]
        //public CategoryMember[] Pages { get; set; }

        [JsonProperty("allimages")]
        public ImageMember[] Images { get; set; }

        /// <summary>
        /// Links - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("alllinks")]
        public LinkMemberContinue Links { get; set; }
    }

    /// <summary>
    /// TextMember - Json-Response Mitglieder
    /// </summary>
    public class TextMember
    {
        /// <summary>
        /// SourceCode - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("*")]
        public string SourceCode { get; set; }
    }


    /// <summary>
    /// TextMember - Json-Response for Users
    /// </summary>
    public class AllUsers
    {
        /// <summary>
        /// SourceCode - Json-Response Users
        /// </summary>
        [JsonProperty("name")]
        public string name { get; set; }
        
        [JsonProperty("userid")]
        public long userid { get; set; }

        
    /// <summary>
    /// TextMember - Json-Response Mitglieder
    /// </summary>
    public class TextMember
    {
        /// <summary>
        /// SourceCode - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("*")]
        public string SourceCode { get; set; }
    }
    }

    /// <summary>
    /// ParseImageMember - Json-Response Mitglieder
    /// </summary>
    public class ParseImageMember
    {
        /// <summary>
        /// Img - Json-Response Mitglieder
        /// </summary>
        public string Img { get; set; }
    }

    /// <summary>
    /// ParseLinkMember - Json-Response Mitglieder
    /// </summary>
    public class ParseLinkMember
    {
        /// <summary>
        /// Ns - Json-Response Mitglieder
        /// </summary>
        public int Ns { get; set; }

        /// <summary>
        /// Title - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("*")]
        public string Title { get; set; }

        /// <summary>
        /// Exists - Json-Response Mitglieder
        /// </summary>
        public string Exists { get; set; }

        /// <summary>
        /// Fromid - Json-Response Mitglieder
        /// </summary>
        public int Fromid { get; set; }
    }

    /// <summary>
    /// Parse - Json-Response Mitglieder
    /// </summary>
    public class Parse
    {
        /// <summary>
        /// Title - Json-Response Mitglieder
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Revid - Json-Response Mitglieder
        /// </summary>
        public int Revid { get; set; }

        /// <summary>
        /// Text - Json-Response Mitglieder
        /// </summary>
        public TextMember Text { get; set; }

        /// <summary>
        /// Images - Json-Response Mitglieder
        /// </summary>
        public string[] Images { get; set; }

        /// <summary>
        /// Links - Json-Response Mitglieder
        /// </summary>
        public ParseLinkMember[] Links { get; set; }

        /// <summary>
        /// Wikitext - Json-Response Mitglieder
        /// </summary>
        public Wikitext Wikitext { get; set; }
    }

    /// <summary>
    /// MediaWiki - Json-Response Mitglieder
    /// </summary>
    public class MediaWiki
    {
        /// <summary>
        /// Query - Json-Response Mitglieder
        /// </summary>
        public Query Query { get; set; }

        /// <summary>
        /// QueryContinue - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("query-continue")]
        public QueryContinue QueryContinue { get; set; }

        /// <summary>
        /// Parse - Json-Response Mitglieder
        /// </summary>
        public Parse Parse { get; set; }

        /// <summary>
        /// Login - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("login")]
        public Login Login { get; set; } 

    }

    /// <summary>
    /// Wikitext - Json-Response Mitglieder
    /// </summary>
    public class Wikitext
    {
        /// <summary>
        /// Contents - Json-Response Mitglieder
        /// </summary>
        [JsonProperty("*")]
        public string Contents { get; set; }
    }

    /// <summary>
    /// Hilfsklasse, um Texte im Abschnitt "== Ratings ==" zu extrahieren
    /// </summary>
    public class RatingsSection
    {
        /// <summary>
        /// Die Dateiname (ohne die "Datei:" Präfix), wo die Ratings abgespeichert sind.
        /// </summary>
        public string RatingsFilename { get; set; }

        /// <summary>
        /// Erzeuge ein RatingsSection Objekt. Produziere den Abschnitttext auf dasis des WikiTexts.
        /// </summary>
        /// <param name="wikitext">WikiText als Basis</param>
        public RatingsSection(string wikitext)
        {
            RatingsFilename = null;

            // in_ratings - the Ratings section stops either when a new section appears or when the end of the input appears
            Regex in_ratings = new Regex(@"== Ratings ==(?<innertext>.*?)(==|$)", RegexOptions.Singleline);
            if (in_ratings.IsMatch(wikitext))
            {
                string innertext = in_ratings.Match(wikitext).Groups["innertext"].Value;

                // in_ratingattrib - identify the filename (not including "Datei:" prefix)
                Regex in_ratingattrib = new Regex(@"RATING=Datei:(?<filename>.*?)\|", RegexOptions.Singleline);
                if (in_ratingattrib.IsMatch(innertext))
                {
                    string filename = in_ratingattrib.Match(innertext).Groups["filename"].Value;
                    RatingsFilename = filename;
                }
            }
        }
    }

    /// <summary>
    /// Hilfsklasse, um Texte im Abschnitt "&lt;imagemap&gt;" zu extrahieren
    /// </summary>
    public class ImageMap
    {
        /// <summary>
        /// Einzelne Zeile im ImageMap Bereich.
        /// </summary>
        public List<ImageMapPiece> Pieces { get; set; }

        /// <summary>
        /// Dateiname wo das Segmentbild gespeichert ist.
        /// </summary>
        public string SegmentFilename { get; set; }

        /// <summary>
        /// Anchortext, der mit dem Segmentbild eingestellt wurde.
        /// </summary>
        public string SegmentAnchortext { get; set; }

        /// <summary>
        /// Erzeuge ein ImageMap Objekt. Konstruriere das ImageMap anhand des Wikitexts.
        /// </summary>
        /// <param name="wikitext">WikiText als Basis</param>
        public ImageMap(string wikitext)
        {
            SegmentFilename = null;
            SegmentAnchortext = null;

            // read each line of wikitext and fill in the Pieces property accordingly
            this.Pieces = new List<ImageMapPiece>();

            Regex in_imagemap = new Regex("<imagemap>(?<innertext>.*?)</imagemap>", RegexOptions.Singleline);
            string lines;
            {
                Match m = in_imagemap.Match(wikitext);
                if (!m.Success)
                {
                    //throw new Exception("Could not find <imagemap> section in the wikitext!");
                    return;
                }
                lines = m.Groups["innertext"].Value;
            }

            // read and classify each line in lines
            Regex blankLine = new Regex(@"^\s*$");
            Regex polyLine = new Regex(@"^\s*poly\s+(?<numlist>[-\d\s]+)\s*\[\[Datei:(?<filename>[^|\]]*)\|?(?<anchortext>[^\]]*)\]\]\s*$");
            Regex circleLine = new Regex(@"^\s*circle\s+(?<numlist>[-\d\s]+)\s*\[\[Datei:(?<filename>[^|\]]*)\|?(?<anchortext>[^\]]*)\]\]\s*$");
            Regex headLine = new Regex(@"^\s*Datei:(?<filename>[^|\]]*)\|?(?<anchortext>[^\]]*)\s*$");
            foreach (string s in lines.Split(new string[] { "\n" }, StringSplitOptions.None))
            {
                if (blankLine.IsMatch(s))
                    continue;
                if (polyLine.IsMatch(s))
                {
                    Match m = polyLine.Match(s);
                    //Console.Out.WriteLine("Got a polyLine: {0}", s);
                    this.Pieces.Add(new ImageMapPiece("poly", m.Groups["numlist"].Value, m.Groups["filename"].Value, m.Groups["anchortext"].Value));
                }
                else if (circleLine.IsMatch(s))
                {
                    Match m = circleLine.Match(s);
                    //Console.Out.WriteLine("Got a circleLine: {0}", s);
                    this.Pieces.Add(new ImageMapPiece("circle", m.Groups["numlist"].Value, m.Groups["filename"].Value, m.Groups["anchortext"].Value));
                }
                else if (headLine.IsMatch(s))
                {
                    Match m = headLine.Match(s);
                    SegmentFilename = m.Groups["filename"].Value;
                    SegmentAnchortext = m.Groups["anchortext"].Value;
                    //Console.Out.WriteLine("found headLine");
                }
                else
                {
                    //Console.Out.WriteLine("Got some other line: {0}", s);
                }
            }
        }
    }

    /// <summary>
    /// Hilfsklasse, um Texte im Abschnitt "&lt;imagemap&gt;" des Wikitexsts zu extrahieren
    /// </summary>
    public class ImageMapPiece
    {
        /// <summary>
        /// Identifiert das Shape, i.e. "poly" oder "circle"
        /// </summary>
        public string Shape { get; set; }

        /// <summary>
        /// Liste von Ints, die allesamt Punkten repräsentieren.
        /// </summary>
        public List<int> PointParams { get; set; }

        /// <summary>
        /// Dateiname der Grafidatei (z.B. Jpeg oder Png), wo das segmentierte Bild abgespeichert wird.
        /// </summary>
        public string Filename;

        /// <summary>
        /// Der Text, der zu diesem Segment gehört. Wird im Web als Link Anchor-Text dargestellt.
        /// </summary>
        public string Anchortext;

        /// <summary>
        /// (deprecated) Erzeuge ein ImageMapPiece Objekt.
        /// </summary>
        /// <param name="wikiline">Einzelne Zeile von WikiText, die bearbeitet wird</param>
        public ImageMapPiece(string wikiline)
        {
            this.Shape = "poly";
            this.PointParams = new List<int>();
            this.PointParams.Add(20);
            this.PointParams.Add(40);
            this.Filename = "foo";
            this.Anchortext = "bar";
        }

        /// <summary>
        /// Erzeuge ein ImageMapPiece Objekt. Konstruriere ein ImageMapPiece Objekt anhand einzelner Zeile von Wikitext (aka wikiline).
        /// </summary>
        /// <param name="shape">Eingabe für Shape Eigenschaft</param>
        /// <param name="pointParamList">Liste von Punkten als String</param>
        /// <param name="filename">Dateiname des Bildsegments</param>
        /// <param name="anchortext">Anchortext des Bildsegments</param>
        public ImageMapPiece(string shape, string pointParamList, string filename, string anchortext)
        {
            Shape = shape;
            PointParams = new List<int>();
            Filename = filename;
            Anchortext = anchortext;

            // read each pointParam of the pointParamList
            Regex pointParamRx = new Regex(@"([-\d]+\s*)");
            Match m = pointParamRx.Match(pointParamList);
            while (m.Success)
            {
                int x = int.Parse(m.Groups[0].Value);
                PointParams.Add(x);
                m = m.NextMatch();
            }
        }
    }
    /***************************************************************************************************/

    /***************************************************************************************************/
    /// <summary>
    /// Hilfsklasse, um Texte vom Wikitext-Abschnitt "== Links ==" zu extrahieren.
    /// </summary>
    public class LinksSection
    {
        /// <summary>
        /// Liste von einzelne Elementen, die zu Links konvertiert werden.
        /// </summary>
        public List<LinksSectionPiece> Pieces { get; set; }

        /// <summary>
        /// Erzeuge ein LinksSection Object. Wird von dem ganzen wikitext Abschnitt gebaut.
        /// </summary>
        /// <param name="wikitext">WikiText als Basis</param>
        public LinksSection(string wikitext)
        {
            // read each line of wikitext and fill in the Pieces property accordingly
            this.Pieces = new List<LinksSectionPiece>();

            // == Links == ... == Ratings ==
            Regex in_links_section = new Regex(@"== Links ==(?<innertext>.*?)(==.*|\s*)$", RegexOptions.Singleline);
            string lines;
            {
                Match m = in_links_section.Match(wikitext);
                if (!m.Success)
                {
                    //throw new Exception("Could not find a suitable \"== Links ==\" section in the wikitext!");
                    return;
                }
                lines = m.Groups["innertext"].Value;
            }

            // read and classify each line in lines
            Regex blankLine = new Regex(@"^\s*$");
            Regex links_line = new Regex(@"^\s*\[\[Links::(?<pagetarget>[^|\]]*)\|?(?<anchortext>[^\]]*)\]\]\s*(<br\s?/?>|$)");

            //Regex polyLine = new Regex(@"^\s*poly\s+(?<numlist>[-\d\s]+)\s*\[\[Datei:(?<filename>[^|\]]*)\|?(?<anchortext>[^\]]*)\]\]\s*$");
            //Regex circleLine = new Regex(@"^\s*circle\s+(?<numlist>[-\d\s]+)\s*\[\[Datei:(?<filename>[^|\]]*)\|?(?<anchortext>[^\]]*)\]\]\s*$");
            foreach (string s in lines.Split(new string[] { "\n" }, StringSplitOptions.None))
            {
                if (blankLine.IsMatch(s))
                    continue;
                if (links_line.IsMatch(s))
                {
                    Match m = links_line.Match(s);
                    //Console.Out.WriteLine("Got a links_line: {0}", s);
                    this.Pieces.Add(new LinksSectionPiece(m.Groups["pagetarget"].Value, m.Groups["anchortext"].Value));
                }
                else
                {
                    //Console.Out.WriteLine("Got some other line: {0}", s);
                }
            }
        }
    }

    /// <summary>
    /// Steht für eine einzelne Zeile im "Links" Bereich von wikitext, 
    /// z.B. "== Links ==\n[[Links::TARGETPAGE|ANCHORTEXT]\n..."
    /// </summary>
    public class LinksSectionPiece
    {
        /// <summary>
        /// Pagetarget definiert den Teil TARGETPAGE Teil wie im Muster "[[Links::TARGETPAGE|ANCHORTEXT]"
        /// </summary>
        public string Pagetarget;

        /// <summary>
        /// Anchortext definiert den Teil ANCHORTEXT Teil wie im Muster "[[Links::TARGETPAGE|ANCHORTEXT]"
        /// </summary>
        public string Anchortext;

        /// <summary>
        /// Erzeuge ein LinksSectionPiece Objekt. Benutzt wird eine Zielseite und den Anchortext.
        /// </summary>
        /// <param name="pagetarget">Zielseite für diesen Link</param>
        /// <param name="anchortext">Anchortext für diesen Link</param>
        public LinksSectionPiece(string pagetarget, string anchortext)
        {
            Pagetarget = pagetarget;
            Anchortext = anchortext;
        }
    }
}
