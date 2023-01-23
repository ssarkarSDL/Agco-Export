using Agco_GetPublicationDetails.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace Agco_GetPublicationDetails
{
    class getPubDetails
    {
        public static string PublicationOutputBasePath = @"D:\Agco\Out\Sample_Publication_extract\";// Publication_List.csv";
        //https://docs.rws.com/992513/69978/tridion-docs-14-sp4/publicationoutput-2-5-find
        public static char tab = '\u0009';
        public static string reduceMultiSpace = @"[ ]{2,}";
        public static string xmlItemRequestedMetadata = "<ishfields>" +
                            "<ishfield name='FTITLE' level='logical' />" +
                            "<ishfield name='DOC-LANGUAGE' level='lng'/>" +
                            "<ishfield name='FRESOLUTION' level='lng'/>" +
                            "<ishfield name='VERSION' level='version'/>" +
    "<ishfield name='FTITLE' level='logical' />" +
    "<ishfield name='FDESCRIPTION' level='logical' />" +
    "<ishfield name='FAGCOCOMPONENTCODE' level='logical' />" +
    "<ishfield name='FAGCOERRORCODE' level='logical' />" +
    "<ishfield name='FAGCOEDTFAILURECODE' level='logical' />" +
    "<ishfield name='FAGCOGLOBALFAILURECODE' level='logical' />" +
    "<ishfield name='FAGCOGLOBALFAILURECODEAREA' level='logical' />" +
    "<ishfield name='FAGCOBRAND' level='version' />" +
    "<ishfield name='FAGCOVEHICLETYPE' level='version' />" +
    "<ishfield name='FAGCOVEHICLESERIES' level='version' />" +
    "<ishfield name='FAGCOCHASSISRANGE' level='version' />" +
    "<ishfield name='FAGCOMODELNAME' level='version' />" +
    "<ishfield name='FAGCOTRANSMISSION' level='version' />" +
    "<ishfield name='FAGCOMARKETINGVERSION' level='version' />" +
    "<ishfield name='FAGCOEMISSIONS' level='version' />" +
    "<ishfield name='FAGCOSEVERITY' level='version' />" +
    "<ishfield name='VERSION' level='version' />" +
    "<ishfield name='FRESOLUTION' level='lng' />" +
    "<ishfield name='FAGCOGLOBALBRANDS' level='version'/>" +
    "<ishfield name='FAGCOGLOBALMACHINETYPES' level='version'/>" +
    "<ishfield name='FAGCOGLOBALMACHINETYPES' level='version'/>" +
    "<ishfield name='FAGCOGLOBALSERIES' level='version' />" +
    "<ishfield name='FAGCOGLOBALMODELS' level='version' />" +
    "<ishfield name='FAGCOGLOBALREGIONS' level='version' />" +
    "<ishfield name='FAGCOGFCTYPE' level='version' />" +
    "<ishfield name='FAGCOGFCMAINAREA' level='version' />" +
    "<ishfield name='FAGCOGFCSUBAREA' level='version' />" +
    "<ishfield name='FAGCOGFCDEFECT' level='version' />" +
    "<ishfield name='FAGCOFAULTCODE' level='version' />" +
    "<ishfield name='FAGCODIAGNOSTICPATH' level='version' />" +
    "<ishfield name='FAGOREPAIRTIME' level='version' />" +
    "<ishfield name='FAGCODIAGNOSTICFUNCTION' level='version' />" +
                        "</ishfields>";
        public static async Task Run(InfoShareWSHelper _WSHelper)
        {
            var pubObj = _WSHelper.GetPublication25Channel();
            try
            {
                
                string xmlRequestedMetadata = "<ishfields>" +
                        "<ishfield name='FTITLE' level='logical'/>" +
                        "<ishfield name='VERSION' level='version'/>" +
                        "<ishfield name='FISHPUBSOURCELANGUAGES' level='version'/>" +
                        "<ishfield name='FISHMASTERREF' level='version'/>" +
                        "<ishfield name='FISHBASELINE' ishvaluetype='element' level='version'/>" +
                        "<ishfield name='FISHBASELINE' ishvaluetype='value' level='version'/>" +
                        "<ishfield name='FISHRESOURCES' level='version'/>" +
                        "<ishfield name='FISHPUBCONTEXT' level='version'/>" +
                        //"<ishfield name='FISHOUTPUTFORMATREF' level='lng' />" +
                    "</ishfields>";
                //var PubDetails1 = pubObj.Find(PublicationOutput25ServiceReference.StatusFilter.ISHNoStatusFilter, null, xmlRequestedMetadata);
                //string tempPath = @"D:\Agco\In\input.xml";
                //if (File.Exists(tempPath))
                //{
                //    File.Delete(tempPath);
                //}
                ////File.WriteAllBytes(tempPath, Convert.FromBase64String(PubDetails1));
                //FileStream fs = File.Create(tempPath);
                //using (var sr = new StreamWriter(fs))
                //{
                //    await sr.WriteLineAsync(PubDetails1);
                //    sr.Close();
                //    sr.Dispose();
                //}
                string PubDetails = string.Empty;
                string _filePath = @"C:\Users\ssarkar\Documents\Agco_Repo\outputFiles\Publication_extract\Publication_List - Sample Export.xml";
                using (var streamReader = new StreamReader(_filePath, Encoding.UTF8))
                {
                    PubDetails = streamReader.ReadToEnd();
                }
                createPblicationFolderStructure_Objects(PubDetails, _WSHelper);
            }
            catch (Exception e)
            {

            }
            //return sPubSpesificDetails;
        }
        public async static void createPblicationFolderStructure_Objects(string content, InfoShareWSHelper _WSHelper)
        {
            try
            {
                var publicationObj = _WSHelper.GetPublication25Channel();
                var baseline25Obj = _WSHelper.GetBaseline25Channel();

                string logicalID = string.Empty;
                string fishPubSourceLanguageDirPath = string.Empty;
                string version = string.Empty;
                string FTITLE = string.Empty;
                string versionDirPath = string.Empty;
                string versionDirName = string.Empty;
                string fishPubSourceLanguage = string.Empty;
                string fishPubContext = string.Empty;
                string fishbaselineID = string.Empty;
                string fishbaselineName = string.Empty;

                string xmlPubRequestedMetadata= "<ishfields>" +
                        "<ishfield name='FTITLE' level='logical' />" +
                        "<ishfield name='FISHPUBSOURCELANGUAGES' level='version'/>" +
                        "<ishfield name='VERSION' level='version'/>" +
"<ishfield name='FISHPUBLICATIONTYPE' level='logical' />" +
"<ishfield name='FDESCRIPTION' level='logical' />" +
"<ishfield name='FAGCOFACTORY' level='logical' />" +
"<ishfield name='FAGCOBRAND' level='version' />" +
"<ishfield name='FAGCOSUBBRAND' level='version' />" +
"<ishfield name='FAGCOMODELNAME' level='version' />" +
"<ishfield name='FAGCOVEHICLESERIES' level='version' />" +
"<ishfield name='FAGCOMARKETINGVERSION' level='version' />" +
"<ishfield name='FAGCOMKTGTRANSMISSION' level='version' />" +
"<ishfield name='FAGCOCHASSISRANGE' level='version' />" +
"<ishfield name='FAGCORELEASEDATE' level='version' />" +
"<ishfield name='FAGCOISSUENUMBER' level='version' />" +
"<ishfield name='FAGCOMARKET' level='version' />" +
"<ishfield name='FAGCOPRINTNUMBER' level='version' />" +
"<ishfield name='FAGCOPARTNUMBER' level='lng' />" +
"<ishfield name='FAGCOBULLETINNR' level='version' />" +
"<ishfield name='FAGCOTRANSOIB' level='lng' />" +
"<ishfield name='FAGCOBARCODE' level='lng' />" +
"<ishfield name='FAGCOGLOBALBRANDS' level='version' />" +
"<ishfield name='FAGCOGLOBALBRANDS' level='version' />" +
"<ishfield name='FAGCOGLOBALMACHINETYPES' level='version' ishvaluetype='value' />" +
"<ishfield name='FAGCOGLOBALMACHINETYPES' level='version' ishvaluetype='id' />" +
"<ishfield name='FAGCOGLOBALSERIES' level='version' ishvaluetype='value' />" +
"<ishfield name='FAGCOGLOBALSERIES' level='version' ishvaluetype='id' />" +
"<ishfield name='FAGCOGLOBALMODELS' level='version' ishvaluetype='value' />" +
"<ishfield name='FAGCOGLOBALMODELS' level='version' ishvaluetype='id' />" +
"<ishfield name='FAGCOGLOBALREGIONS' level='version' ishvaluetype='value' />" +
"<ishfield name='FAGCOGLOBALREGIONS' level='version' ishvaluetype='id' />" +
"<ishfield name='FAGCOGFCTYPE' level='version' ishvaluetype='value' />" +
"<ishfield name='FAGCOGFCTYPE' level='version' ishvaluetype='id' />" +
"<ishfield name='FAGCOGFCMAINAREA' level='version' ishvaluetype='id' />" +
"<ishfield name='FAGCOGFCMAINAREA' level='version' ishvaluetype='value' />" +
"<ishfield name='FAGCOGFCMAINAREA' level='version' ishvaluetype='id' />" +
"<ishfield name='FAGCOGFCSUBAREA' level='version' ishvaluetype='value' />" +
"<ishfield name='FAGCOGFCSUBAREA' level='version' ishvaluetype='id' />" +
"<ishfield name='FAGCOGFCDEFECT' level='version' ishvaluetype='value' />" +
"<ishfield name='FAGCOGFCDEFECT' level='version' ishvaluetype='id' />" +
//"<ishfield name='FISHOUTPUTFORMATREF' level='lng' />" +
                    "</ishfields>";


                XmlDocument xDoc1 = new XmlDocument();
                xDoc1.LoadXml(content);
                XmlNodeList nodes1 = xDoc1.SelectNodes("//ishobject");
                foreach (XmlNode _eachPubObj in nodes1)
                {
                    logicalID = _eachPubObj.Attributes["ishref"].InnerText.ToString();
                    string pubDirTitle = "Publication (" + logicalID + ")";
                    string pubDirPath = PublicationOutputBasePath + pubDirTitle;
                    if (!Directory.Exists(pubDirPath))
                    {
                        Directory.CreateDirectory(pubDirPath);
                    }
                    version= _eachPubObj.SelectSingleNode(".//ishfield[@name='VERSION']").InnerText.Trim();
                    FTITLE = _eachPubObj.SelectSingleNode(".//ishfield[@name='FTITLE']").InnerText.Trim();
                    FTITLE = string.Join(" ", Regex.Split(FTITLE, @"(?:\r\n|\n|\r|\t)"));
                    FTITLE = Regex.Replace(FTITLE, @"\s+", " ").Replace(@"\", "_").Replace(@"/", "_");
                    FTITLE = Regex.Replace(FTITLE, @"^[A - Za - z0 - 9_.] +$", "_");
                    versionDirName = "Version " + version;
                    versionDirPath = pubDirPath + @"\" + versionDirName;
                    if (!Directory.Exists(versionDirPath))
                    {
                        Directory.CreateDirectory(versionDirPath);
                    }
                    fishPubSourceLanguage = _eachPubObj.SelectSingleNode(".//ishfield[@name='FISHPUBSOURCELANGUAGES']").InnerText.Trim();
                    fishPubSourceLanguageDirPath = versionDirPath + @"\" + fishPubSourceLanguage;
                    if (!Directory.Exists(fishPubSourceLanguageDirPath))
                    {
                        Directory.CreateDirectory(fishPubSourceLanguageDirPath);
                    }
                    fishPubContext= _eachPubObj.SelectSingleNode(".//ishfield[@name='FISHPUBCONTEXT']").InnerText.Trim();

                    if (Directory.Exists(versionDirPath) && Directory.Exists(fishPubSourceLanguageDirPath))
                    {
                        string pubMeta = publicationObj.RetrieveMetadata(new string[] { logicalID },PublicationOutput25ServiceReference.StatusFilter.ISHNoStatusFilter, null, xmlPubRequestedMetadata);
                        XmlDocument xDoc3 = new XmlDocument();
                        xDoc3.LoadXml(pubMeta);
                        XmlNodeList nodes3 = xDoc3.SelectNodes("//ishobject");
                        bool flag = true;
                        foreach(XmlNode node in nodes3)
                        {
                            string pubOutputversion = string.Empty;
                            string fishPubOutputSourceLanguage = string.Empty;
                            pubOutputversion = node.SelectSingleNode(".//ishfield[@name='VERSION']").InnerText.Trim();

                            fishPubOutputSourceLanguage = node.SelectSingleNode(".//ishfield[@name='FISHPUBSOURCELANGUAGES']").InnerText.Trim();
                            if (version.Equals(pubOutputversion) && fishPubSourceLanguage.Equals(fishPubOutputSourceLanguage))
                            {
                                if (flag)
                                {
                                    await GeneratePubMeta(node, version, fishPubSourceLanguage, fishPubSourceLanguageDirPath);
                                    flag = false;
                                }
                                
                            }
                            long pubLangRef = long.Parse(node.Attributes["ishlngref"].InnerText.ToString());
                            string subOutDirPath = fishPubSourceLanguageDirPath + @"\Output";
                            if (!Directory.Exists(subOutDirPath))
                            {
                                Directory.CreateDirectory(subOutDirPath);
                            }
                            string fileName = FTITLE + "=" + logicalID + "=" + version + "=" + fishPubSourceLanguage;
                           // string fileName = FTITLE + "=" + GUID + "=" + ishversionref + "=" + docLng + "=" + FRESOLUTION;

                            await DownloadOutput(pubLangRef, subOutDirPath, fishPubSourceLanguage, fileName, _WSHelper);
                        }
                        await GenerateConditionCsv(logicalID, fishPubContext, fishPubSourceLanguageDirPath);
                        fishbaselineID = _eachPubObj.SelectSingleNode(".//ishfield[@name='FISHBASELINE'and@ishvaluetype='element']").InnerText.Trim();
                        fishbaselineName = _eachPubObj.SelectSingleNode(".//ishfield[@name='FISHBASELINE'and@ishvaluetype='value']").InnerText.Trim();
                        string xmlBaseLine = string.Empty;
                        
                        string usedObjectOutput = baseline25Obj.GetReport(fishbaselineID, null, new string[] { fishPubSourceLanguage }, null, null, new string[] { "high" });
                        //baseline25Obj.GetBaseline(out xmlBaseLine, fishbaselineID, null);
                        await generateBaseLineReport(usedObjectOutput, fishPubSourceLanguageDirPath);
                        XmlDocument xDoc2 = new XmlDocument();
                        xDoc2.LoadXml(usedObjectOutput);
                        XmlNodeList mapNodes = xDoc2.SelectNodes("//object[@type='ISHMasterDoc']");
                        
                        string bookMapGUID =_eachPubObj.SelectSingleNode(".//ishfield[@name='FISHMASTERREF']").InnerText.Trim();
                        long lngref = 0;
                        List<long> subMapLngRef = new List<long>();
                        foreach (XmlNode _eachPubObj1 in mapNodes)
                        {
                            if (_eachPubObj1.Attributes["ref"]!=null)
                            {
                                string mapid = _eachPubObj1.Attributes["ref"].InnerText.ToString();
                                if (mapid == bookMapGUID)
                                {
                                    lngref = long.Parse(_eachPubObj1.SelectSingleNode(".//reportitem").Attributes["lngref"].Value.Trim());
                                }
                                else
                                {
                                    subMapLngRef.Add(long.Parse(_eachPubObj1.SelectSingleNode(".//reportitem").Attributes["lngref"].Value.Trim()));
                                }
                            }
                        }
                        await generateMainMap(bookMapGUID, fishPubSourceLanguageDirPath, lngref, _WSHelper);
                        if(subMapLngRef.Count>0)
                        {
                            string subMapDirPath = fishPubSourceLanguageDirPath + @"\SubMaps";
                            if (!Directory.Exists(subMapDirPath))
                            {
                                Directory.CreateDirectory(subMapDirPath);
                            }
                            await generateSubMaps(subMapLngRef, subMapDirPath, _WSHelper);

                        }
                        List<long> TopicLangRefs = new List<long>();
                        XmlNodeList topicNodes = xDoc2.SelectNodes("//object[@type='ISHModule']");

                        foreach (XmlNode _eachPubObj1 in topicNodes)
                        {
                            if (_eachPubObj1.Attributes["ref"] != null && _eachPubObj1.Attributes["type"] != null)
                            {
                                string topicid = _eachPubObj1.Attributes["ref"].InnerText.ToString();
                                string topicType = _eachPubObj1.Attributes["type"].InnerText.ToString();
                                if (topicType.ToLower().Equals("ishmodule"))
                                {
                                    if (_eachPubObj1.SelectSingleNode(".//reportitem").Attributes["lngref"] != null)
                                    {
                                        TopicLangRefs.Add(long.Parse(_eachPubObj1.SelectSingleNode(".//reportitem").Attributes["lngref"].Value.Trim()));
                                    }
                                }
                            }
                        }
                        if (TopicLangRefs.Count > 0)
                        {
                            string subTopicDirPath = fishPubSourceLanguageDirPath + @"\Topics";
                            //fishPubSourceLanguageDirPath = versionDirPath + @"\" + fishPubSourceLanguage;
                            if (!Directory.Exists(subTopicDirPath))
                            {
                                Directory.CreateDirectory(subTopicDirPath);
                            }
                            await createTopics(TopicLangRefs.ToArray(), subTopicDirPath, fishPubSourceLanguage, _WSHelper);
                        }
                        List<long> IlusLangRefs = new List<long>();
                        XmlNodeList IlusNodes = xDoc2.SelectNodes("//object[@type='ISHIllustration']");
                        foreach (XmlNode _eachPubObj1 in IlusNodes)
                        {
                            if (_eachPubObj1.Attributes["ref"] != null && _eachPubObj1.Attributes["type"] != null)
                            {
                                string topicid = _eachPubObj1.Attributes["ref"].InnerText.ToString();
                                string topicType = _eachPubObj1.Attributes["type"].InnerText.ToString();
                                if (topicType.ToLower().Equals("ishillustration"))
                                {
                                    if (_eachPubObj1.SelectSingleNode(".//reportitem").Attributes["lngref"] != null)
                                    {
                                        IlusLangRefs.Add(long.Parse(_eachPubObj1.SelectSingleNode(".//reportitem").Attributes["lngref"].Value.Trim()));

                                    }

                                }
                            }
                        }
                        if (IlusLangRefs.Count > 0)
                        {
                            string subIluscDirPath = fishPubSourceLanguageDirPath + @"\Images";
                            //fishPubSourceLanguageDirPath = versionDirPath + @"\" + fishPubSourceLanguage;
                            if (!Directory.Exists(subIluscDirPath))
                            {
                                Directory.CreateDirectory(subIluscDirPath);
                            }
                            await createishillustrations(IlusLangRefs.ToArray(), subIluscDirPath, fishPubSourceLanguage, _WSHelper);
                        }

                        List<long> IshTempLangRefs = new List<long>();
                        XmlNodeList IshTempNodes = xDoc2.SelectNodes("//object[@type='ISHTemplate']");
                        foreach (XmlNode _eachPubObj1 in IshTempNodes)
                        {
                            if (_eachPubObj1.Attributes["ref"] != null && _eachPubObj1.Attributes["type"] != null)
                            {
                                string topicid = _eachPubObj1.Attributes["ref"].InnerText.ToString();
                                string topicType = _eachPubObj1.Attributes["type"].InnerText.ToString();
                                if (topicType.ToLower().Equals("ishtemplate"))
                                {
                                    if (_eachPubObj1.SelectSingleNode(".//reportitem").Attributes["lngref"] != null)
                                    {
                                        IshTempLangRefs.Add(long.Parse(_eachPubObj1.SelectSingleNode(".//reportitem").Attributes["lngref"].Value.Trim()));

                                    }

                                }
                            }
                        }
                        if (IshTempLangRefs.Count > 0)
                        {
                            string subTempDirPath = fishPubSourceLanguageDirPath + @"\Others (templates)";
                            //fishPubSourceLanguageDirPath = versionDirPath + @"\" + fishPubSourceLanguage;
                            if (!Directory.Exists(subTempDirPath))
                            {
                                Directory.CreateDirectory(subTempDirPath);
                            }
                            await createishtemplates(IshTempLangRefs.ToArray(), subTempDirPath, fishPubSourceLanguage, _WSHelper);
                        }
                        List<long> IshLibLangRefs = new List<long>();
                        XmlNodeList IshLibNodes = xDoc2.SelectNodes("//object[@type='ISHLibrary']");
                        foreach (XmlNode _eachPubObj1 in IshLibNodes)
                        {
                            if (_eachPubObj1.Attributes["ref"] != null && _eachPubObj1.Attributes["type"] != null)
                            {
                                string topicid = _eachPubObj1.Attributes["ref"].InnerText.ToString();
                                string topicType = _eachPubObj1.Attributes["type"].InnerText.ToString();
                                if (topicType.ToLower().Equals("ishlibrary"))
                                {
                                    if (_eachPubObj1.SelectSingleNode(".//reportitem").Attributes["lngref"] != null)
                                    {
                                        IshLibLangRefs.Add(long.Parse(_eachPubObj1.SelectSingleNode(".//reportitem").Attributes["lngref"].Value.Trim()));

                                    }

                                }
                            }
                        }
                        if (IshLibLangRefs.Count > 0)
                        {
                            string subLibDirPath = fishPubSourceLanguageDirPath + @"\Libraries";
                            //fishPubSourceLanguageDirPath = versionDirPath + @"\" + fishPubSourceLanguage;
                            if (!Directory.Exists(subLibDirPath))
                            {
                                Directory.CreateDirectory(subLibDirPath);
                            }
                            await createISHLibrary(IshLibLangRefs.ToArray(), subLibDirPath, fishPubSourceLanguage, _WSHelper);
                        }

                    }
                    // fishbaselineID= _eachPubObj.SelectSingleNode(".//ishfield[@name='fishbaseline'/@ishvaluetype='element']").InnerText.Trim();

                }

            }
            catch(Exception e)
            {

            }
           // return PubSpesificDetails;
        }

        public async static Task DownloadOutput(long pubLongref, string subLibDirPath, string docLng, string fileName, InfoShareWSHelper _WSHelper)
        {
            var PublicationClient = _WSHelper.GetPublication25Channel();
            XDocument myPubObjInfo = new XDocument();
            myPubObjInfo = XDocument.Parse(PublicationClient.GetDataObjectInfoByIshLngRef(pubLongref));
            string ishLngRef = string.Empty;
            string fileext = string.Empty;
            string edRef = string.Empty;
            string size = string.Empty;
            int chksize = 8000000;
            Byte[] bytesInStream;

            if (myPubObjInfo.Root.Descendants().Count() > 0)
            {
                //Retrieve and store blob in bloks of 200KB = 200000 bytes
                ishLngRef = myPubObjInfo.Root.Element("ishdataobject").Attribute("ishlngref").Value;
                fileext = myPubObjInfo.Root.Element("ishdataobject").Attribute("fileextension").Value;
                edRef = myPubObjInfo.Root.Element("ishdataobject").Attribute("ed").Value;
                size = myPubObjInfo.Root.Element("ishdataobject").Attribute("size").Value;
                //  Console.WriteLine("myLNGRef = {0},{1},{2},{3}", ishLngRef, fileext, edRef, size);
            }
            string filePath = subLibDirPath + @"\" + fileName + "." + fileext;
            if (!File.Exists(filePath))
            {
                
            int numberOfChunks = int.Parse(size) / chksize;

            int lastChunkSize = int.Parse(size) % chksize;
            // drawTextProgressBar(0, numberOfChunks);
            Console.WriteLine($"Downloading {numberOfChunks} chunks");
            if (lastChunkSize > 0)
            {
                numberOfChunks++;
            }

            long offset = 0;
            List<byte> downloadedBlob = new List<byte> { };
            for (int i = 0; i < numberOfChunks; i++)
            {
                //define offset based upon the number of repetition
                offset = chksize * i;
                // drawTextProgressBar(i, numberOfChunks);
                Console.WriteLine("Downloading Chunck {0} of {1}", i, numberOfChunks);
                if (i == numberOfChunks - 1)
                {
                    chksize = lastChunkSize;
                    // drawTextProgressBar(numberOfChunks, numberOfChunks);
                    Console.WriteLine("Downloading Chunck {0} of {1}", numberOfChunks, numberOfChunks);
                }

                byte[] downloadedChunk = null;
                // Console.WriteLine("Downloading chunk ‘{0}’ of ‘{1}’, offset set to ‘{2}’", i + 1, numberOfChunks, offset);
                bool timeout = false;
                int noRetries = 0;
                do
                {//PublicationOutput25ServiceReference
                    //https://agcodev01.sdlproducts.com/ISHWS/Wcf/API25/PublicationOutput.svc
                    try
                    {
                        PublicationClient.GetNextDataObjectChunkByIshLngRef(long.Parse(ishLngRef), edRef, ref offset, ref chksize, out downloadedChunk);
                        // PublicationClient.GetNextDataObjectChunkByIshLngRefAsync()
                        //Concat all retrieved chunks in on object
                        downloadedBlob.AddRange(new List<byte>(downloadedChunk));
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Warning, unable to download the output format. Retrying again");
                        timeout = true;
                        ++noRetries;
                        // _logger.writeWrn("Unable to download output Format", $"Unable to download output format {filename}. NO of retries {noRetries}", null);
                    }
                }
                while (timeout && noRetries < 4);

            }

            File.WriteAllBytes(filePath, downloadedBlob.ToArray());
        }
        }
        public async static Task createISHLibrary(long[] lngref, string subLibDirPath,string docLng, InfoShareWSHelper _WSHelper)
        {
            try
            {
                var document25Obj = _WSHelper.GetDocumentObj25Channel();
                
                string templates = document25Obj.RetrieveObjectsByIshLngRefs(lngref, null, xmlItemRequestedMetadata);
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(templates);
                XmlNodeList _dataNodes = xDoc.SelectNodes("//ishobject");
                foreach (XmlNode _eachTopicObj in _dataNodes)
                {
                    string ishversionref = string.Empty;
                   // string docLng = string.Empty;
                    string FTITLE = string.Empty;
                    string FRESOLUTION = string.Empty;
                    string GUID = string.Empty;
                    string topicFileType = string.Empty;
                    string objContent = string.Empty;
                    string metFilecontent = string.Empty;


                    if (_eachTopicObj.Attributes["ishref"] != null)
                    {
                        GUID = _eachTopicObj.Attributes["ishref"].InnerText;
                    }
                    XmlNode _dataNode = _eachTopicObj.SelectSingleNode("//ishdata");
                    if (_dataNode != null)
                    {
                        topicFileType = _dataNode.Attributes["fileextension"].InnerText.ToString();
                        objContent = _dataNode.InnerText.ToString();
                    }
                    if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='VERSION']") != null)
                    {
                        ishversionref = _eachTopicObj.SelectSingleNode(".//ishfield[@name='VERSION']").InnerText;
                    }
                    //if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='DOC-LANGUAGE']") != null)
                    //{
                    //    docLng = _eachTopicObj.SelectSingleNode(".//ishfield[@name='DOC-LANGUAGE']").InnerText;
                    //}
                    if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='FTITLE']") != null)
                    {
                        FTITLE = _eachTopicObj.SelectSingleNode(".//ishfield[@name='FTITLE']").InnerText;
                    }
                    if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='FRESOLUTION']") != null)
                    {
                        FRESOLUTION = _eachTopicObj.SelectSingleNode(".//ishfield[@name='FRESOLUTION']").InnerText;
                    }

                    FTITLE = string.Join(" ", Regex.Split(FTITLE, @"(?:\r\n|\n|\r|\t)"));
                    FTITLE = Regex.Replace(FTITLE, @"\s+", " ").Replace(@"\", "_").Replace(@"/", "_");
                    FTITLE = Regex.Replace(FTITLE, @"^[A - Za - z0 - 9_.] +$", "_");
                    string fileName = FTITLE + "=" + GUID + "=" + ishversionref + "=" + docLng + "=" + FRESOLUTION;

                    string ilusLocalFilePath = subLibDirPath + @"\" + fileName + "." + topicFileType;
                    if (File.Exists(ilusLocalFilePath))
                    {
                        File.Delete(ilusLocalFilePath);
                    }
                    File.WriteAllBytes(ilusLocalFilePath, Convert.FromBase64String(objContent));
                    _eachTopicObj.RemoveChild(_dataNode);
                    metFilecontent = _eachTopicObj.OuterXml.ToString();
                    string metFileNamePath = subLibDirPath + @"\" + fileName + ".met";
                    if (File.Exists(metFileNamePath))
                    {
                        File.Delete(metFileNamePath);
                    }
                    FileStream fs = File.Create(metFileNamePath);
                    using (var sr = new StreamWriter(fs))
                    {
                        await sr.WriteLineAsync(metFilecontent);
                        sr.Close();
                        sr.Dispose();
                    }
                }
            }
            catch(Exception e)
            {

            }
            
        }
        public async static Task createishtemplates(long[] lngref, string subTempDirPath,string docLng, InfoShareWSHelper _WSHelper)
        {
            try
            {
                var document25Obj = _WSHelper.GetDocumentObj25Channel();
                
                string templates = document25Obj.RetrieveObjectsByIshLngRefs(lngref, null, xmlItemRequestedMetadata);
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(templates);
                XmlNodeList _dataNodes = xDoc.SelectNodes("//ishobject");
                foreach (XmlNode _eachTopicObj in _dataNodes)
                {
                    string ishversionref = string.Empty;
                    //string docLng = string.Empty;
                    string FTITLE = string.Empty;
                    string FRESOLUTION = string.Empty;
                    string GUID = string.Empty;
                    string topicFileType = string.Empty;
                    string objContent = string.Empty;
                    string metFilecontent = string.Empty;


                    if (_eachTopicObj.Attributes["ishref"] != null)
                    {
                        GUID = _eachTopicObj.Attributes["ishref"].InnerText;
                    }
                    XmlNode _dataNode = _eachTopicObj.SelectSingleNode("//ishdata");
                    if (_dataNode != null)
                    {
                        topicFileType = _dataNode.Attributes["fileextension"].InnerText.ToString();
                        objContent = _dataNode.InnerText.ToString();
                    }
                    if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='VERSION']") != null)
                    {
                        ishversionref = _eachTopicObj.SelectSingleNode(".//ishfield[@name='VERSION']").InnerText;
                    }
                    //if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='DOC-LANGUAGE']") != null)
                    //{
                    //    docLng = _eachTopicObj.SelectSingleNode(".//ishfield[@name='DOC-LANGUAGE']").InnerText;
                    //}
                    if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='FTITLE']") != null)
                    {
                        FTITLE = _eachTopicObj.SelectSingleNode(".//ishfield[@name='FTITLE']").InnerText;
                    }
                    if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='FRESOLUTION']") != null)
                    {
                        FRESOLUTION = _eachTopicObj.SelectSingleNode(".//ishfield[@name='FRESOLUTION']").InnerText;
                    }

                    FTITLE = string.Join(" ", Regex.Split(FTITLE, @"(?:\r\n|\n|\r|\t)"));
                    FTITLE = Regex.Replace(FTITLE, @"\s+", " ").Replace(@"\", "_").Replace(@"/", "_");
                    FTITLE = Regex.Replace(FTITLE, @"^[A - Za - z0 - 9_.] +$", "_");
                    string fileName = FTITLE + "=" + GUID + "=" + ishversionref + "=" + docLng + "=" + FRESOLUTION;

                    string ilusLocalFilePath = subTempDirPath + @"\" + fileName + "." + topicFileType;
                    if (File.Exists(ilusLocalFilePath))
                    {
                        File.Delete(ilusLocalFilePath);
                    }
                    File.WriteAllBytes(ilusLocalFilePath, Convert.FromBase64String(objContent));
                    _eachTopicObj.RemoveChild(_dataNode);
                    metFilecontent = _eachTopicObj.OuterXml.ToString();
                    string metFileNamePath = subTempDirPath + @"\" + fileName + ".met";
                    if (File.Exists(metFileNamePath))
                    {
                        File.Delete(metFileNamePath);
                    }
                    FileStream fs = File.Create(metFileNamePath);
                    using (var sr = new StreamWriter(fs))
                    {
                        await sr.WriteLineAsync(metFilecontent);
                        sr.Close();
                        sr.Dispose();
                    }
                }

            }
            catch (Exception e)
            {

            }
            

        }
        public async static Task createishillustrations(long[] lngref, string subIluscDirPath, string docLng, InfoShareWSHelper _WSHelper)
        {
            try
            {
                var document25Obj = _WSHelper.GetDocumentObj25Channel();
                
                string illustrations = document25Obj.RetrieveObjectsByIshLngRefs(lngref, null, xmlItemRequestedMetadata);
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(illustrations);
                XmlNodeList _dataNodes = xDoc.SelectNodes("//ishobject");
                foreach (XmlNode _eachTopicObj in _dataNodes)
                {
                    string ishversionref = string.Empty;
                    //string docLng = string.Empty;
                    string FTITLE = string.Empty;
                    string FRESOLUTION = string.Empty;
                    string GUID = string.Empty;
                    string topicFileType = string.Empty;
                    string objContent = string.Empty;
                    string metFilecontent = string.Empty;


                    if (_eachTopicObj.Attributes["ishref"] != null)
                    {
                        GUID = _eachTopicObj.Attributes["ishref"].InnerText;
                    }
                    XmlNode _dataNode = _eachTopicObj.SelectSingleNode("//ishdata");
                    if (_dataNode != null)
                    {
                        topicFileType = _dataNode.Attributes["fileextension"].InnerText.ToString();
                        objContent = _dataNode.InnerText.ToString();
                    }
                    if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='VERSION']") != null)
                    {
                        ishversionref = _eachTopicObj.SelectSingleNode(".//ishfield[@name='VERSION']").InnerText;
                    }
                    //if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='DOC-LANGUAGE']") != null)
                    //{
                    //    docLng = _eachTopicObj.SelectSingleNode(".//ishfield[@name='DOC-LANGUAGE']").InnerText;
                    //}
                    if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='FTITLE']") != null)
                    {
                        FTITLE = _eachTopicObj.SelectSingleNode(".//ishfield[@name='FTITLE']").InnerText;
                    }
                    if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='FRESOLUTION']") != null)
                    {
                        FRESOLUTION = _eachTopicObj.SelectSingleNode(".//ishfield[@name='FRESOLUTION']").InnerText;
                    }

                    FTITLE = string.Join(" ", Regex.Split(FTITLE, @"(?:\r\n|\n|\r|\t)"));
                    FTITLE = Regex.Replace(FTITLE, @"\s+", " ").Replace(@"\", "_").Replace(@"/", "_");
                    FTITLE = Regex.Replace(FTITLE, @"^[A - Za - z0 - 9_.] +$", "_");
                    string fileName = FTITLE + "=" + GUID + "=" + ishversionref + "=" + docLng + "=" + FRESOLUTION;

                    string ilusLocalFilePath = subIluscDirPath + @"\" + fileName + "." + topicFileType;
                    if (File.Exists(ilusLocalFilePath))
                    {
                        File.Delete(ilusLocalFilePath);
                    }
                    File.WriteAllBytes(ilusLocalFilePath, Convert.FromBase64String(objContent));
                    //_dataNode.RemoveAll();
                    _eachTopicObj.RemoveChild(_dataNode);
                    metFilecontent = _eachTopicObj.OuterXml.ToString();//.ChildNodes[0].InnerXml.ToString();
                    string metFileNamePath = subIluscDirPath + @"\" + fileName + ".met";
                    if (File.Exists(metFileNamePath))
                    {
                        File.Delete(metFileNamePath);
                    }
                    FileStream fs = File.Create(metFileNamePath);
                    using (var sr = new StreamWriter(fs))
                    {
                        await sr.WriteLineAsync(metFilecontent);
                        sr.Close();
                        sr.Dispose();
                    }
                }

            }
            catch (Exception e)
            {

            }
            
        }
        public async static Task createTopics(long[] lngref, string subTopicDirPath, string docLng, InfoShareWSHelper _WSHelper)
        {
            try
            {
                var document25Obj = _WSHelper.GetDocumentObj25Channel();
                
                string topics = document25Obj.RetrieveObjectsByIshLngRefs(lngref, null, xmlItemRequestedMetadata);
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(topics);
                XmlNodeList _dataNodes = xDoc.SelectNodes("//ishobject");

                //string mainMapFileType = _dataNode.Attributes["fileextension"].InnerText.ToString();

                foreach (XmlNode _eachTopicObj in _dataNodes)
                {
                    string ishversionref = string.Empty;
                    //string docLng = string.Empty;
                    string FTITLE = string.Empty;
                    string FRESOLUTION = string.Empty;
                    string GUID = string.Empty;
                    string topicFileType = string.Empty;
                    string objContent = string.Empty;
                    string metFilecontent = string.Empty;

                    if (_eachTopicObj.Attributes["ishref"] != null)
                    {
                        GUID = _eachTopicObj.Attributes["ishref"].InnerText;
                    }
                    XmlNode _dataNode = _eachTopicObj.SelectSingleNode("//ishdata");
                    if (_dataNode != null)
                    {
                        topicFileType = _dataNode.Attributes["fileextension"].InnerText.ToString();
                        objContent = _dataNode.InnerText.ToString();
                    }
                    if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='VERSION']") != null)
                    {
                        ishversionref = _eachTopicObj.SelectSingleNode(".//ishfield[@name='VERSION']").InnerText;
                    }
                    //if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='DOC-LANGUAGE']") != null)
                    //{
                    //    docLng = _eachTopicObj.SelectSingleNode(".//ishfield[@name='DOC-LANGUAGE']").InnerText;
                    //}
                    if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='FTITLE']") != null)
                    {
                        FTITLE = _eachTopicObj.SelectSingleNode(".//ishfield[@name='FTITLE']").InnerText;
                    }
                    if (_eachTopicObj.SelectSingleNode(".//ishfield[@name='FRESOLUTION']") != null)
                    {
                        FRESOLUTION = _eachTopicObj.SelectSingleNode(".//ishfield[@name='FRESOLUTION']").InnerText;
                    }

                    FTITLE = string.Join(" ", Regex.Split(FTITLE, @"(?:\r\n|\n|\r|\t)"));
                    FTITLE = Regex.Replace(FTITLE, @"\s+", " ").Replace(@"\", "_").Replace(@"/", "_");
                    FTITLE = Regex.Replace(FTITLE, @"^[A - Za - z0 - 9_.] +$", "_");
                    string fileName = FTITLE + "=" + GUID + "=" + ishversionref + "=" + docLng + "=" + FRESOLUTION;

                    string topicLocalFilePath = subTopicDirPath + @"\" + fileName + "." + topicFileType;
                    if (File.Exists(topicLocalFilePath))
                    {
                        File.Delete(topicLocalFilePath);
                    }
                    File.WriteAllBytes(topicLocalFilePath, Convert.FromBase64String(objContent));
                    _eachTopicObj.RemoveChild(_dataNode);
                    metFilecontent = _eachTopicObj.OuterXml.ToString();
                    string metFileNamePath= subTopicDirPath + @"\" + fileName + ".met";
                    if (File.Exists(metFileNamePath))
                    {
                        File.Delete(metFileNamePath);
                    }
                    FileStream fs = File.Create(metFileNamePath);
                    using (var sr = new StreamWriter(fs))
                    {
                        await sr.WriteLineAsync(metFilecontent);
                        sr.Close();
                        sr.Dispose();
                    }
                }

            }
            catch (Exception e)
            {

            }
            
        }

        public async static Task generateSubMaps(List<long> lngrefs, string fishPubSourceLanguageDirPath, InfoShareWSHelper _WSHelper)
        {
            try
            {
                var document25Obj = _WSHelper.GetDocumentObj25Channel();


                string mainMap = document25Obj.RetrieveObjectsByIshLngRefs(lngrefs.ToArray(), null, xmlItemRequestedMetadata);
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(mainMap);
                //XmlNode _dataNode = xDoc.SelectSingleNode("//ishdata");
                
                   
                XmlNodeList nodes1 = xDoc.SelectNodes("//ishobject");
                string ishversionref = string.Empty;
                string docLng = string.Empty;
                string FTITLE = string.Empty;
                string FRESOLUTION = string.Empty;
                string mainMapFileType = string.Empty;
                string objContent = string.Empty;
                foreach (XmlNode _eachPubObj in nodes1)
                {
                    //string mainMapFileType = _dataNode.Attributes["fileextension"].InnerText.ToString();
                    XmlNode _dataNode = _eachPubObj.SelectSingleNode("//ishdata");
                    if (_dataNode != null)
                    { 
                        mainMapFileType = _dataNode.Attributes["fileextension"].InnerText.ToString();

                        objContent = _dataNode.InnerText.ToString();
                    }
                    string mapId= _eachPubObj.Attributes["ishref"].InnerText.ToString();
                    if (_eachPubObj.SelectSingleNode(".//ishfield[@name='VERSION']") != null)
                    {
                        ishversionref = _eachPubObj.SelectSingleNode(".//ishfield[@name='VERSION']").InnerText;
                    }
                    if (_eachPubObj.SelectSingleNode(".//ishfield[@name='DOC-LANGUAGE']") != null)
                    {
                        docLng = _eachPubObj.SelectSingleNode(".//ishfield[@name='DOC-LANGUAGE']").InnerText;
                    }
                    if (_eachPubObj.SelectSingleNode(".//ishfield[@name='FTITLE']") != null)
                    {
                        FTITLE = _eachPubObj.SelectSingleNode(".//ishfield[@name='FTITLE']").InnerText;
                    }
                    if (_eachPubObj.SelectSingleNode(".//ishfield[@name='FRESOLUTION']") != null)
                    {
                        FRESOLUTION = _eachPubObj.SelectSingleNode(".//ishfield[@name='FRESOLUTION']").InnerText;
                    }
                    FTITLE = string.Join(" ", Regex.Split(FTITLE, @"(?:\r\n|\n|\r|\t)"));
                    FTITLE = Regex.Replace(FTITLE, @"\s+", " ").Replace(@"\", "_").Replace(@"/", "_");
                    FTITLE = Regex.Replace(FTITLE, @"^[A - Za - z0 - 9_.] +$", "_");
                    string fileName = FTITLE + "=" + mapId +  "=" + ishversionref + "=" + docLng + "=" + FRESOLUTION;
                    string mainMaplocalFilePath = fishPubSourceLanguageDirPath + @"\" + fileName + "." + mainMapFileType;
                    if (File.Exists(mainMaplocalFilePath))
                    {
                        File.Delete(mainMaplocalFilePath);
                    }
                    File.WriteAllBytes(mainMaplocalFilePath, Convert.FromBase64String(objContent));
                    string metFileName = fileName + ".met";
                    string metFilePath = fishPubSourceLanguageDirPath + @"\" + metFileName;
                    if (File.Exists(metFilePath))
                    {
                        File.Delete(metFilePath);
                    }
                    FileStream fs = File.Create(metFilePath);
                    //XmlNodeList nodes2 = xDoc.SelectNodes("//ishfields");
                    using (var sr = new StreamWriter(fs))
                    {
                        await sr.WriteLineAsync(nodes1[0].FirstChild.ToString());
                        sr.Close();
                        sr.Dispose();
                    }
                    Console.WriteLine("Submap xml and met file: " + metFilePath);
                }
                    

               // }
            }
            catch (Exception e)
            {

            }
        }

        public async static Task generateMainMap(string bookMapGUID, string fishPubSourceLanguageDirPath,long lngref, InfoShareWSHelper _WSHelper)
        {
            try
            {
                var document25Obj = _WSHelper.GetDocumentObj25Channel();
                

                string mainMap = document25Obj.RetrieveObjectsByIshLngRefs(new long[] { lngref }, null, xmlItemRequestedMetadata);
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(mainMap);
                XmlNode _dataNode = xDoc.SelectSingleNode("//ishdata");
                if (_dataNode != null)
                {
                    string mainMapFileType = _dataNode.Attributes["fileextension"].InnerText.ToString();

                    string objContent = _dataNode.InnerText.ToString();
                    XmlNodeList nodes1 = xDoc.SelectNodes("//ishobject");
                    string ishversionref = string.Empty;
                    string docLng = string.Empty;
                    string FTITLE = string.Empty;
                    string FRESOLUTION = string.Empty;
                    foreach (XmlNode _eachPubObj in nodes1)
                    {
                        if (_eachPubObj.SelectSingleNode(".//ishfield[@name='VERSION']") != null)
                        {
                            ishversionref = _eachPubObj.SelectSingleNode(".//ishfield[@name='VERSION']").InnerText;
                        }
                        if (_eachPubObj.SelectSingleNode(".//ishfield[@name='DOC-LANGUAGE']") != null)
                        {
                            docLng = _eachPubObj.SelectSingleNode(".//ishfield[@name='DOC-LANGUAGE']").InnerText;
                        }
                        if (_eachPubObj.SelectSingleNode(".//ishfield[@name='FTITLE']") != null)
                        {
                            FTITLE = _eachPubObj.SelectSingleNode(".//ishfield[@name='FTITLE']").InnerText;
                        }
                        if (_eachPubObj.SelectSingleNode(".//ishfield[@name='FRESOLUTION']") != null)
                        {
                            FRESOLUTION = _eachPubObj.SelectSingleNode(".//ishfield[@name='FRESOLUTION']").InnerText;
                        }
                    }
                    FTITLE = string.Join(" ", Regex.Split(FTITLE, @"(?:\r\n|\n|\r|\t)"));
                    FTITLE = Regex.Replace(FTITLE, @"\s+", " ").Replace(@"\", "_").Replace(@"/", "_");
                    FTITLE = Regex.Replace(FTITLE, @"^[A - Za - z0 - 9_.] +$", "_");
                    string fileName = FTITLE + "=" + bookMapGUID + "=" + ishversionref + "=" + docLng + "=" + FRESOLUTION;
                    string mainMaplocalFilePath = fishPubSourceLanguageDirPath + @"\" + fileName + "." + mainMapFileType;
                    if (File.Exists(mainMaplocalFilePath))
                    {
                        File.Delete(mainMaplocalFilePath);
                    }
                    File.WriteAllBytes(mainMaplocalFilePath, Convert.FromBase64String(objContent));
                    string metFileName = fileName + ".met";
                    string metFilePath = fishPubSourceLanguageDirPath + @"\" + metFileName;
                    if (File.Exists(metFilePath))
                    {
                        File.Delete(metFilePath);
                    }
                    FileStream fs = File.Create(metFilePath);
                    //XmlNodeList nodes2 = xDoc.SelectNodes("//ishfields");
                    using (var sr = new StreamWriter(fs))
                    {
                        await sr.WriteLineAsync(nodes1[0].FirstChild.ToString());
                        sr.Close();
                        sr.Dispose();
                    }
                    Console.WriteLine("bookmap xml and met file: " + metFilePath);
                }
            }
            catch(Exception e)
            {

            }
            
            
        }
        public async static Task generateBaseLineReport(string usedObjectOutput, string fishPubSourceLanguageDirPath)
        {
            try
            {
                string fileName = fishPubSourceLanguageDirPath + @"\Baseline.xml";
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                FileStream fs = File.Create(fileName);
                using (var sr = new StreamWriter(fs))
                {
                    await sr.WriteLineAsync(usedObjectOutput.ToString());
                    sr.Close();
                    sr.Dispose();
                }
                Console.WriteLine("baseline xml created: " + fileName);
            }
            catch(Exception e)
            {

            }
            
        }

        public async static Task GenerateConditionCsv(string logicalID, string fishPubContext, string fishPubSourceLanguageDirPath)
        {
            try
            {
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(fishPubContext);
                string temp_xmlfilePath = @"D:\Agco\In\XSL\myfilename_" + logicalID + @".xml";
                if(File.Exists(temp_xmlfilePath))
                {
                    File.Delete(temp_xmlfilePath);
                }
                xdoc.Save(temp_xmlfilePath);
                if(File.Exists(temp_xmlfilePath))
                {
                    var doc = XDocument.Load(temp_xmlfilePath);
                    var myDictionary = doc.Root.Elements()
                                       .ToDictionary(a => (string)a.Attribute("name"),
                                                     a => (string)a.Attribute("value"));
                    if (myDictionary.Count > 0)
                    {
                        string filepath = fishPubSourceLanguageDirPath + @"\Conditions.csv";
                        if (Directory.Exists(fishPubSourceLanguageDirPath))
                        {
                            FileStream fs = File.Create(filepath);
                        }
                        string newContent = string.Empty;
                        foreach (KeyValuePair<string, string> entry in myDictionary)
                        {
                            newContent += entry.Key + "=" + entry.Value;
                            using (StreamWriter sw = File.AppendText(filepath))
                            {
                                sw.WriteLine(newContent);
                                sw.Close();
                                sw.Dispose();
                            }
                            
                        }
                        Console.WriteLine("PubContext created in csv: " + filepath);
                    }
                    
                }
                
            }
            catch(Exception e)
            {

            }
            
           
        }
        public async static Task GeneratePubMeta(XmlNode _eachPubObj, string version, string fishPubSourceLanguage, string fishPubSourceLanguageDirPath)
        {
            try
            {
                //XmlDocument xDoc1 = new XmlDocument();
                //xDoc1.LoadXml(PubMeta);
                //XmlNodeList nodes1 = xDoc1.SelectNodes("//ishobject");
                
                string fileName = fishPubSourceLanguageDirPath + @"\Publication.met";
                //foreach (XmlNode _eachPubObj in nodes1)
                //{
                    
                        if (File.Exists(fileName))
                        {
                            File.Delete(fileName);
                        }
                        FileStream fs = File.Create(fileName);
                        using (var sr = new StreamWriter(fs))
                        {
                            await sr.WriteLineAsync(_eachPubObj.OuterXml.ToString());
                            sr.Close();
                            sr.Dispose();
                        }
                        Console.WriteLine("PubMeta created: " + fileName);
                        //break;
                   // }

                //}
            
            }
            catch (Exception e)
            {

            }
        }
    }
}
