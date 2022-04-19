using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;
using UniquePro.Entities.Common;
using UniquePro.DAO.Common;

namespace RaxUploader
{
    public class Application : ServiceControl
    {
        private dynamic configuration;

        private List<FileScanner> scanners;
        
        public bool Start(HostControl hostControl)
        {

            configuration = Newtonsoft.Json.JsonConvert.DeserializeObject<ExpandoObject>(File.ReadAllText("configuration.json"));
            
            string failFolder = (string)configuration.failFolder;
            
            Console.WriteLine("[{0} {1}] - Connect Server", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString());

            if (!Directory.Exists(failFolder))
                Directory.CreateDirectory(failFolder);

            List<object> folders = configuration.folders;

            scanners = new List<FileScanner>();

            int delay = (int)configuration.scanInterval / folders.Count;

            Console.Title = "UniqueUploader:" + configuration.computername;
            
            foreach (var folder in folders)
            {
                FileScanner scanner = new FileScanner(folder.ToString(), failFolder, (int)configuration.scanInterval , configuration.heightfolder , configuration.computername);
                scanner.OnFoundNewFile += Scanner_OnFoundNewFile;
                scanners.Add(scanner);
                scanner.Start();

                Thread.Sleep(delay);

            }

            return true;
        }

        //private async void Scanner_OnFoundNewFile(object sender, FixtureFile e)
        private void Scanner_OnFoundNewFile(object sender, FixtureFile e )
        {            
            string ServerPath = "";
            InterfaceDAO objInterfaceDAO = new InterfaceDAO();

            try
            {
                
                Console.WriteLine("[{2} {0}] - found {1}", Thread.CurrentThread.ManagedThreadId, e.FileInfo.FullName ,DateTime.Now.ToString ());
                e.Validate();
                Console.WriteLine("[{0} {1}] - Validated", Thread.CurrentThread.ManagedThreadId , DateTime.Now.ToString ());

                //e.Pick();

                // insert into database
                //await Task.Factory.StartNew(() => 
                //{
                try
                {

                    //Task.Delay(36000);

                    string filedate = e.FileInfo.Name;            
                    
                    
                    String[] Temp = filedate.ToLower().Split('_');
                   
                    InterfaceHisDataM objIntrafaceM = new InterfaceHisDataM();
             
                    String[] Result = e.Content.Split(',');
                   
                    objIntrafaceM.FileName = e.FileInfo.FullName;
                 
                    for (int i = 0; i <= Result.Length - 1; i++)
                    {
                        objIntrafaceM.ReferenceNo = Result[0].ToString()??"";
                        objIntrafaceM.Lab = Result[1].ToString() ?? "";
                        objIntrafaceM.OrderType = Result[2].ToString() ?? "";
                        objIntrafaceM.HN = Result[3].ToString() ?? "";
                        objIntrafaceM.Title = Result[4].ToString() ?? "";
                        objIntrafaceM.Firstname  = Result[5].ToString() ?? "";
                        objIntrafaceM.Lastname = Result[6].ToString() ?? "";
                        objIntrafaceM.Sex = Result[7].ToString() ?? "";
                        objIntrafaceM.BirthDate = Result[8].ToString() ?? "";
                        objIntrafaceM.RequestDate = Result[9].ToString() ?? "";
                        objIntrafaceM.RequestTime  = Result[10].ToString() ?? "";
                        objIntrafaceM.RequestDoctor = Result[11].ToString() ?? "";
                        objIntrafaceM.SpecimenCollectionDate = Result[12].ToString() ?? "";
                        objIntrafaceM.SpecimenCollectionTime = Result[13].ToString() ?? "";
                        objIntrafaceM.SpecimenCollectionPerson = Result[14].ToString() ?? "";
                        objIntrafaceM.RecieveDate = Result[15].ToString() ?? "";
                        objIntrafaceM.RecieveTime = Result[16].ToString() ?? "";
                        objIntrafaceM.RecievePerson = Result[17].ToString() ?? "";
                        objIntrafaceM.SpecialRequest = Result[18].ToString() ?? "";
                        objIntrafaceM.Ward = Result[19].ToString() ?? "";
                        objIntrafaceM.Specimen = Result[20].ToString() ?? "";
                        objIntrafaceM.UnderlyingDisease= Result[21].ToString() ?? "";
                        objIntrafaceM.DiagnosisCode = Result[22].ToString() ?? "";
                        objIntrafaceM.AntimicrocialNameUsed = Result[23].ToString() ?? "";
                        objIntrafaceM.Note = Result[24].ToString() ?? "";
                        objIntrafaceM.TestCode = Result[25].ToString().Trim() ?? "";
                      
                    }                   

                    try
                        {
                            objInterfaceDAO.InsertInterfaceTable(objIntrafaceM);
                            //objResult = objMachine.UpdateAOIforTri(objResult);
                            //Console.WriteLine("[{0}] - call UpdateAOIforTri:serialno", objResult.PanelID + ": Result:" + objResult.Status + ",DefectCode:" + objResult.DefectCode);
                        Console.WriteLine("[{0}] - call Upload file completed :serverpath:" + ServerPath, e.FileInfo.FullName);
                        }
                        catch (Exception ex)
                        {
                            string Datetime = DateTime.Now.ToString("dd/mm/yyyy");
                            string destination = Path.Combine((string)configuration.backupfolder  + "\\" + Datetime, e.FileInfo.Name);
                            string sError = "";
                            sError = ex.Message + "File:" + destination;

                            //objMachine.WriteMachineErrorLogs(sError);

                            if (File.Exists(e.FileInfo.FullName))
                            {
                                if (File.Exists(destination))
                                    File.Delete(destination);

                                if (Directory.Exists((string)configuration.backupfolder) == false)
                                {
                                    Directory.CreateDirectory((string)configuration.backupfolder + "\\" + Datetime);
                                } 

                                File.Move(e.FileInfo.FullName, destination);
                             }
                                
                            Console.WriteLine("[{1}{0}] - Err", DateTime.Now.ToString(), sError);

                        //Console.WriteLine("[{1}{0}] - Err",DateTime.Now.ToString (), ex.Message);
                        }                                           
                        //if (File.Exists(e.FileInfo.FullName))
                        //{                            
                        //    try
                        //    {                            
                        //        File.Delete(e.FileInfo.FullName);                               
                        //    }
                        //    catch
                        //    {
                        //        throw new Exception();
                        //    }
                        //}
                                                

                    }
                   catch (Exception er)
                   {
                    String sError = "";
                    
                        string destination = Path.Combine((string)configuration.failFolder, e.FileInfo.Name);

                        sError = er.Message + "File:" + destination;

                        //objMachine.WriteMachineErrorLogs(sError);

                        if (File.Exists(e.FileInfo.FullName))
                            {
                                if (File.Exists(destination))
                                    File.Delete(destination);

                                File.Move(e.FileInfo.FullName, destination);
                            }

                    
                        Console.WriteLine("[{1}{0}] - Err", DateTime.Now.ToString (),sError);

                        //if (File.Exists(e.FileInfo.FullName))
                        //{
                        //        Console.WriteLine("[{0}] - Err", er.Message);
                        //        string destination = Path.Combine((string)configuration.failFolder, e.FileInfo.Name);
                        //        File.Move(e.FileInfo.FullName, destination );

                        //        ServerPath = objMachine.UploadErrorFile(destination, "HSG_AOI");

                        //        Console.WriteLine("[{0}] - Err", er.Message);

                        //        File.Delete(e.FileInfo.FullName);


                        //        //try
                        //        //{
                        //        //        if (File.Exists(e.FileInfo.FullName) == true)
                        //        //        {
                        //        //            Console.WriteLine("[{0}] - Err", er.Message);
                        //        //            string destination = Path.Combine((string)configuration.failFolder, e.FileInfo.Name);
                        //        //            File.Move(e.FileInfo.FullName, destination);

                        //        //            ServerPath = objMachine.UploadErrorFile(destination, "HSG_AOI");

                        //        //            Console.WriteLine("[{0}] - Err", er.Message);

                        //        //            File.Delete(e.FileInfo.FullName);

                        //        //        }
                        //        //    }
                        //        //catch
                        //        //{
                        //        //    throw new Exception();
                        //        //}
                        //    }


                    }
                //});
                string Datetime1 = DateTime.Now.ToString("ddMMyyyy");
                string destination1 = Path.Combine((string)configuration.backupfolder + "\\" + Datetime1, e.FileInfo.Name);
                

                if (File.Exists(e.FileInfo.FullName))
                {
                    if (File.Exists(destination1))
                        File.Delete(destination1);

                    if (Directory.Exists(destination1 ) == false)
                    {
                        Directory.CreateDirectory((string)configuration.backupfolder + "\\" + Datetime1);
                    }

                    File.Move(e.FileInfo.FullName, destination1);
                }

                Console.WriteLine("[{1}{0}] - Status", DateTime.Now.ToString(), "OK");

                e.Pick();

                Console.WriteLine("[{0} {1}] - Delete File", Thread.CurrentThread.ManagedThreadId, DateTime.Now.ToString());

            }
            catch (FormatException ex)
            {
                Console.WriteLine("Validated"  + ex.Message );
                /*
                string destination = Path.Combine((string)configuration.failFolder, e.FileInfo.Name);


                if (File.Exists(e.FileInfo.FullName))
                {
                    if (File.Exists(destination))
                        File.Delete(destination);
                    try { 
                    File.Move(e.FileInfo.FullName, destination);
                    }catch
                    {

                    }
                }
                */

            }

            catch (Exception ex)
            {
                Console.WriteLine("Validated" + ex.Message);
            }
            finally {
                //string destination = Path.Combine((string)configuration.backupfolder, e.FileInfo.Name);
                //if (File.Exists(e.FileInfo.FullName))
                //{
                //    try { 
                //    if (File.Exists(destination))
                //        File.Delete(destination);

                //   File.Move(e.FileInfo.FullName, destination);
                //    }
                //    catch
                //    {

                //    }
                //}
            }
        }

        public bool Stop(HostControl hostControl)
        {
            foreach (var scanner in scanners)
                scanner.Stop();

            return true;
        }
    }
}
