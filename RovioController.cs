using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Drawing;
using System.IO;


namespace RovioLib
{
    /// <summary>
    /// Class for stroring connection settings
    /// </summary>
    public class RovioSettings
    {
        /// <summary>
        ///  Network Credential to access Rovio
        /// </summary>
        public NetworkCredential RovioCredentials;
        
        /// <summary>
        /// Address to acces Rovio
        /// </summary>
        public string RovioAddress;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username">Username to access Rovio</param>
        /// <param name="password">Password to access Rovio</param>
        /// <param name="address">Address to acces Rovio</param>
        public RovioSettings(string username, string password, string address)
        {
            this.RovioCredentials = new NetworkCredential(username, password);
            this.RovioAddress = address;
        }
    }

    /// <summary>
    /// Class for performing http requests
    /// </summary>
    public class RovioWebClient
    {
        private RovioSettings settings;
   
        /// <summary>
        /// Constructor for RovioWebClient
        /// </summary>
        /// <param name="settings"></param>
        public RovioWebClient(RovioSettings settings)
        {
            this.settings = settings;
        }
       
        /// <summary>
        /// Web request for string data to the Rovio API 
        /// </summary>
        /// <param name="cmd">Command of the Rovio API to execute</param>
        /// <returns></returns>
        public string RequestString(string cmd)
        {
            WebClient  wc = new WebClient();
            wc.Credentials = settings.RovioCredentials;
            wc.BaseAddress = settings.RovioAddress;
            Uri targetUri = new Uri(new Uri(wc.BaseAddress), cmd);
            try
            {
                return wc.DownloadString(targetUri);
            }
            catch (WebException e)
            {
                //TODO: Do something
                return e.Message;
            }
        }

        /// <summary>
        /// Web request for byte data to the Rovio API 
        /// </summary>
        /// <param name="cmd">Command of the Rovio API to execute</param>
        /// <returns></returns>
        public byte[] RequestData(string cmd)
        {
            WebClient wc = new WebClient();
            wc.Credentials = settings.RovioCredentials;
            wc.BaseAddress = settings.RovioAddress;
            Uri targetUri = new Uri(new Uri(wc.BaseAddress), cmd);
            try
            {
                return wc.DownloadData(targetUri);
            }
            catch (WebException e)
            {
                //TODO: Do something
                return null;
            }
        }
    }
    /// <summary>
    /// Class for accessing Rovio API
    /// </summary>
    
    public class RovioController
    {
        private RovioWebClient rwc;
        
        /// <summary>
        /// Constructor for RovioController object 
        /// </summary>
        /// <param name="username">Username to access Rovio</param>
        /// <param name="password">Password to access Rovio</param>
        /// <param name="address">Address to acces Rovio</param>
        public RovioController(string username, string password, string address)
        {
            this.rwc = new RovioWebClient(new RovioSettings(username, password, address));
        }
        /// <summary>
        /// Constructor for RovioController object
        /// </summary>
        /// <param name="settings">RovioSettings object</param>
        public RovioController(RovioSettings settings)
        {
            this.rwc = new RovioWebClient(settings);
        }

        /// <summary>
        /// Generates a report from libNS module that provides Rovio’s current status.
        /// </summary>
        /// <returns></returns>
        public string GetReport() 
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=1");
        }

        /// <summary>
        /// Start recording a path.
        /// </summary>
        /// <returns></returns>
        public string StartRecoding()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=2");
        }

        /// <summary>
        /// Terminates recording of a path without storing it to flash memory.
        /// </summary>
        /// <returns></returns>
        public string AbortRecording()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=3");
        }

        /// <summary>
        /// Stops the recoding of a path and stores it in flash memory; javascript will give default name if user does not provide one.
        /// </summary>
        /// <param name="PathName">name of the path</param>
        /// <returns>Response code</returns>
        public string StopRecording(string PathName)
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=4&name=" + PathName);
        }

        /// <summary>
        /// Deletes specified path.
        /// </summary>
        /// <param name="PathName">name of the path</param>
        /// <returns>Response code</returns>
        public string Deletepath(string PathName)
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=5&name=" + PathName);
        }

        /// <summary>
        /// Returns a list of paths stored in the robot.
        /// </summary>
        /// <returns></returns>
        public string GetPathList()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=6");
        }

        /// <summary>
        /// Replays a stored path from closest point to the end; If the NorthStar signal is lost, it stops.
        /// </summary>
        /// <remarks>In API 1.2 there is no mention of PathName parameter</remarks>
        /// <param name="PathName"></param>
        /// <returns></returns>
        public string PlayPathForward(string PathName)
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=7&name=" + PathName);
        }

        /// <summary>
        /// Replays a stored path from closest point to the beginning; If NorthStar signal is lost it stops.
        /// </summary>
        /// <remarks>In API 1.2 there is no mention of PathName parameter</remarks>
        /// <param name="PathName"></param>
        /// <returns></returns>
        public string PlayPathBackward(string PathName)
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=8&name=" + PathName);
        }

        /// <summary>
        /// Stop playing a path.
        /// </summary>
        /// <returns></returns>
        public string StopPlaying()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=9");
        }
        
        /// <summary>
        /// Pause the robot and waits for a new pause or stop command.
        /// </summary>
        /// <returns></returns>
        public string PausePlaying()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=10");
        }

        /// <summary>
        /// Rename the old path.
        /// </summary>
        /// <param name="OldPathName"></param>
        /// <param name="NewPathName"></param>
        /// <returns></returns>
        public string RenamePath(string OldPathName, string NewPathName)
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=11&name=" + OldPathName + "&newname=" + NewPathName);
        }

        /// <summary>
        /// Drive to home location in front of charging station.
        /// </summary>
        /// <returns></returns>
        public string GoHome()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=12");
        }

        /// <summary>
        /// Drive to home location in front of charging station and dock.
        /// </summary>
        /// <returns></returns>
        public string GoHomeAndDock()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=13");
        }


        /// <summary>
        /// Define current position as home location in front of charging station.
        /// </summary>
        /// <returns></returns>
        public string UpdateHomePosition()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=14");
        }

        /// <summary>
        /// Change homing, docking and driving parameters – speed for driving commands.
        /// <remarks>NOT IMPLEMENTED</remarks>
        /// </summary>
        /// <returns></returns>
        public string SetTuningParameters()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=15");
        }

        
        /// <summary>
        /// Returns homing, docking and driving parameters.
        /// </summary>
        /// <returns></returns>
        public string GetTuningParameters()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=16");
        }

        /// <summary>
        /// Stops whatever it was doing and resets to idle state.
        /// </summary>
        /// <returns></returns>
        public string ResetNavStateMachine()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=17");
        }

        
        /// <summary>
        /// Accepts manual driving commands.
        /// </summary>
        /// <param name="drive"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public string ManualDrive(int drive, int speed)
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=18&drive=" + drive.ToString() + "&speed=" + speed.ToString() );
        }


        /// <summary>
        ///  Turning on/off IR detector.
        /// </summary>
        /// <param name="state">0 - off, 1 - on</param>
        /// <returns></returns>
        public string SetIRState(int state)
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=19&IR="+state.ToString());
        }


        /// <summary>
        /// Returns MCU report including wheel encoders and IR obstacle avoidance.
        /// </summary>
        /// <returns></returns>
        public string GetMCUReport()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=20");
        }

        /// <summary>
        /// Deletes all paths in the robot’s Flash memory.
        /// </summary>
        /// <returns></returns>
        public string ClearAllPaths()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=21");
        }


        /// <summary>
        /// Reports navigation state. 
        /// </summary>
        /// <remarks>Name changed from GetStatus (in API two fucntions with same name)</remarks>
        /// <returns></returns>
        public string GetNavStatus() 
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=22");
        }


        /// <summary>
        /// Stores parameter in the robot’s Flash memory.
        /// </summary>
        /// <param name="index">0 – 19</param>
        /// <param name="value">32bit signed integer</param>
        /// <returns></returns>
        public string SaveParameter(long index, long value)
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=23&index=" + index.ToString() + "&value=" + value.ToString() );
        }


        /// <summary>
        /// Read parameter in the robot’s Flash memory.
        /// </summary>
        /// <param name="index">0 – 19</param>
        /// <returns></returns>
        public string ReadParameter(long index)
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=24&index=" + index.ToString() );
        }

        /// <summary>
        /// Returns string version of libNS and NS sensor.
        /// </summary>
        /// <returns></returns>
        public string GetLibNSVersion() 
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=25");
        }


        /// <summary>
        /// Emails current image or if in path recording mode sets an action.
        /// </summary>
        /// <param name="email">email address (hello@gmail.com)</param>
        /// <returns></returns>
        public string EmailImage(string email)
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=26&email=" +email );
        }


        /// <summary>
        /// Clears home location in the robot's Flash memory.
        /// </summary>
        /// <returns></returns>
        public string ResetHomeLocation()
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=27");
        }

        /// <summary>
        /// The basic command for acquiring MJPEG.
        /// <remarks>NOT IMPLEMENTED</remarks>
        /// </summary>
        /// <returns></returns>
        public string GetData()
        {
            return rwc.RequestString("GetData.cgi");
        }

        
        /// <summary>
        /// The basic command for acquiring Image.
        /// </summary>
        /// <returns>Bitmap</returns>
        public Bitmap GetImage()
        {
            byte[] buf = rwc.RequestData("Jpeg/CamImg0000.jpg");
            return new Bitmap(Image.FromStream(new MemoryStream(buf)));
        }

        /// <summary>
        /// Change the resolution setting of camera's images.
        /// </summary>
        /// <param name="ResType">Camera supports 4 types of resolution:0 - {176, 144}1 - {352, 288}2 - {320, 240} (Default)3 - {640, 480}</param>
        /// <returns></returns>
        public string ChangeResolution(int ResType)
        {
            return rwc.RequestString("ChangeResolution.cgi?ResType=" + ResType.ToString() );
        }

        /// <summary>
        /// Change the quality setting of camera's images. (only available with MPEG4)
        /// </summary>
        /// <param name="Ratio">0 – 2 (representing low, medium and high quality respectively)</param>
        /// <returns></returns>
        public string ChangeCompressRatio(int Ratio)
        {
            return rwc.RequestString("ChangeCompressRatio.cgi?Ratio=" + Ratio.ToString() );
        }


        /// <summary>
        /// Change the frame rate setting of camera's images.
        /// </summary>
        /// <param name="Framerate">2 – 32 frame per seconds respectively</param>
        /// <returns></returns>
        public string ChangeFramerate(int Framerate)
        {
            return rwc.RequestString("ChangeFramerate.cgi?Framerate=" + Framerate.ToString());
        }


        /// <summary>
        /// Change the brightness setting of camera's images.
        /// </summary>
        /// <param name="Brightness">0 - 6 (The lower the value is, the dimmer the image is)</param>
        /// <returns></returns>
        public string ChangeBrightness(int Brightness)
        {
            return rwc.RequestString("ChangeBrightness.cgi?Brightness=" + Brightness.ToString() );
        }

        /// <summary>
        /// Change the Speaker Volume setting of camera.
        /// </summary>
        /// <param name="SpeakerVolume">0 - 31 (The lower the value is, the lower the speaker volume is)</param>
        /// <returns></returns>
        public string ChangeSpeakerVolume(int SpeakerVolume)
        {
            return rwc.RequestString("ChangeSpeakerVolume.cgi?SpeakerVolume=" + SpeakerVolume.ToString() );
        }

        /// <summary>
        /// Change the Mic Volume setting of IP_Cam.
        /// </summary>
        /// <param name="MicVolume">0 - 31 (The lower the value is, the lower the mic volume is)</param>
        /// <returns></returns>
        public string ChangeMicVolume(int MicVolume)
        {
            return rwc.RequestString("ChangeMicVolume.cgi?MicVolume=" + MicVolume.ToString() );
        }

        /// <summary>
        /// Change camera sensor’s settings.
        /// </summary>
        /// <param name="Frequency">50 – 50Hz, 60 – 60Hz, 0 – Auto detect</param>
        /// <returns></returns>
        public string SetCamera(int Frequency)
        {
            return rwc.RequestString("SetCamera.cgi?Frequency=" + Frequency.ToString() );
        }

        /// <summary>
        /// Get the camera sensor’s settings.
        /// </summary>
        /// <returns></returns>
        public string GetCamera() 
        {
            return rwc.RequestString("GetCamera.cgi");
        }


        /// <summary>
        /// Get the username who sent this HTTP request.
        /// </summary>
        /// <param name="ShowPrivilege"></param>
        /// <returns>Privilege = 0 (for common user),Privilege = 1 (for super user),(Always returns 0 if it is in Non-authorization mode under SetUserCheck.cgi)</returns>
        public string GetMyself(bool ShowPrivilege)
        {
            return rwc.RequestString("GetMyself.cgi?ShowPrivilege=" + ShowPrivilege.ToString() );
        }


        /// <summary>
        /// Add a user or change the password for existed user.
        /// </summary>
        /// <param name="User"></param>
        /// <param name="Pass"></param>
        /// <returns></returns>
        public string SetUser(string User, string Pass)
        {
            return rwc.RequestString("SetUser.cgi?User=" + User + "&Pass=" + Pass);
        }

        /// <summary>
        /// Delete a user account.
        /// </summary>
        /// <param name="User"></param>
        /// <returns></returns>
        public string DelUser(string User)
        {
            return rwc.RequestString("DelUser.cgi?User=" + User);
        }

        /// <summary>
        /// Get the users list of IP Camera.
        /// </summary>
        /// <param name="ShowPrivilege"></param>
        /// <returns></returns>
        public string GetUser(bool ShowPrivilege)
        {
            return rwc.RequestString("GetUser.cgi?ShowPrivilege=" + ShowPrivilege.ToString() );
        }

        /// <summary>
        /// Enable or disable user authorization check.
        /// </summary>
        /// <param name="Check"></param>
        /// <returns></returns>
        public string SetUserCheck(bool Check)
        {
            return rwc.RequestString("SetUserCheck.cgi?Check=" + Check.ToString());
        }

        /// <summary>
        /// Set server time zone and time.
        /// </summary>
        /// <param name="Sec1970">seconds since "00:00:00 1/1/1970".</param>
        /// <param name="TimeZone">Time zone in minutes. (e.g. Beijing is GMT+08:00, TimeZone = -480)</param>
        /// <returns></returns>
        public string SetTime(long Sec1970, int TimeZone)
        {
            return rwc.RequestString("SetTime.cgi?Sec1970=" + Sec1970.ToString() + "&TimeZone=" + TimeZone.ToString() );
        }


        /// <summary>
        /// Get current IP Camera's time zone and time.
        /// </summary>
        /// <returns></returns>
        public string GetTime() 
        {
            return rwc.RequestString("GetTime.cgi");
        }


        /// <summary>
        /// Set a logo string on the image.
        /// </summary>
        /// <param name="showstring">time - time, date - date,ver - version</param>
        /// <param name="pos">0 – top left, 1 – top right, 2 – bottom left, 3 – bottom right</param>
        /// <returns></returns>
        public string SetLogo(string showstring, int pos)
        {
            return rwc.RequestString("SetLogo.cgi?showstring=" + showstring + "&pos=" + pos.ToString() );
        }
        
        
        
        /// <summary>
        /// Get a logo string on the image.
        /// </summary>
        /// <returns></returns>
        public string GetLogo() 
        {
            return rwc.RequestString("GetLogo.cgi");
        }

        /// <summary>
        /// Get IP settings.
        /// </summary>
        /// <param name="Interface">eth1, wlan0</param>
        /// <returns></returns>
        public string GetIP(string Interface)
        {
            return rwc.RequestString("GetIP.cgi?Interface=" + Interface);
        }

        /// <summary>
        /// Get WiFi settings.
        /// </summary>
        /// <returns></returns>
        public string GetWlan() 
        {
            return rwc.RequestString("GetWlan.cgi");
        }

        /// <summary>
        /// Get DDNS settings.
        /// </summary>
        /// <returns></returns>
        public string GetDDNS() 
        {
            return rwc.RequestString("GetDDNS.cgi");
        }

        /// <summary>
        /// Set Mac address.
        /// </summary>
        /// <param name="MAC">Mac address</param>
        /// <returns></returns>
        public string SetMac(string MAC)
        {
            return rwc.RequestString("SetMac.cgi?MAC=" + MAC );
        }
        
        /// <summary>
        /// Get Mac address.
        /// </summary>
        /// <returns></returns>
        public string GetMac() 
        {
            return rwc.RequestString("GetMac.cgi");
        }

        /// <summary>
        /// Get HTTP server's settings.
        /// </summary>
        /// <returns></returns>
        public string GetHttp() 
        {
            return rwc.RequestString("GetHttp.cgi");
        }

        /// <summary>
        /// Get email settings.
        /// </summary>
        /// <returns></returns>
        public string GetMail() 
        {
            return rwc.RequestString("GetMail.cgi");
        }

        /// <summary>
        /// Send an email with IPCam images.
        /// </summary>
        /// <returns></returns>
        public string SendMail() 
        {
            return rwc.RequestString("SendMail.cgi");
        }


        /// <summary>
        /// Set camera's name.
        /// </summary>
        /// <param name="CameraName"></param>
        /// <returns></returns>
        public string SetName(string CameraName)
        {
            return rwc.RequestString("SetName.cgi?CameraName=" + CameraName );
        }

        /// <summary>
        /// Get camera's name.
        /// </summary>
        /// <returns></returns>
        public string GetName() 
        {
            return rwc.RequestString("GetName.cgi");
        }
        
        /// <summary>
        /// Get run-time status of Rovio.
        /// </summary>
        /// <returns></returns>
        public string GetStatus() 
        {
            return rwc.RequestString("GetStatus.cgi");
        }

        /// <summary>
        /// Get Rovio’s system logs information.
        /// </summary>
        /// <returns></returns>
        public string GetLog() 
        {
            return rwc.RequestString("GetLog.cgi");
        }

        /// <summary>
        /// Get Rovio’s base firmware version, Rovio also has a UI version and a NS2 version this function only get the base OS version.
        /// </summary>
        /// <returns></returns>
        public string GetVer() 
        {
            return rwc.RequestString("GetVer.cgi");
        }

        /// <summary>
        /// Change all settings to factory-default.
        /// </summary>
        /// <returns></returns>
        public string SetFactoryDefault() 
        {
            return rwc.RequestString("SetFactoryDefault.cgi");
        }

        /// <summary>
        /// Reboot Rovio.
        /// </summary>
        /// <returns></returns>
        public string Reboot() 
        {
            return rwc.RequestString("Reboot.cgi");
        }

        /// <summary>
        /// Set the media format.
        /// </summary>
        /// <param name="Audio">0 – 4</param>
        /// <param name="Video">0 – 1</param>
        /// <returns></returns>
        public string SetMediaFormat(int Audio,int Video)
        {
            return rwc.RequestString("SetMediaFormat.cgi?Audio=" + Audio.ToString() + "&Video=" + Video.ToString() );
        }

        /// <summary>
        /// Get the media format.
        /// </summary>
        /// <returns></returns>
        public string GetMediaFormat() 
        {
            return rwc.RequestString("GetMediaFormat.cgi");
        }

        /// <summary>
        /// Turn off or turn on Rovio head light.
        /// </summary>
        /// <param name="Value">0 - Off, 1 - On</param>
        /// <returns></returns>
        public string SetHeadLight(int Value)
        {
            return rwc.RequestString("rev.cgi?Cmd=nav&action=19&LIGHT=" + Value.ToString() );
        }

    }
}
