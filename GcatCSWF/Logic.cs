using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.AccountManagement;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GcatCSWF
{

    class Logic
    {
        /// <summary>
        /// Formから呼び出される論理部
        /// </summary>
        /// 

        private static DirectoryEntry GetDirectoryEntry(string path)
        {
            path = "LDAP://" + path;
            return new DirectoryEntry(path, null, null, AuthenticationTypes.Secure);
        }


        public static void GetNodeData(JObject resultJson)
        {
            resultJson.Add("Hostname", System.Environment.MachineName);
            resultJson.Add("SystemType", (String)FromWMI.GetWMIPropaty("Win32_ComputerSystem", "SystemType"));
            resultJson.Add("Domain", (String)FromWMI.GetWMIPropaty("Win32_ComputerSystem", "Domain"));
            resultJson.Add("Manufacturer", (String)FromWMI.GetWMIPropaty("Win32_ComputerSystem", "Manufacturer"));
            resultJson.Add("Model", (String)FromWMI.GetWMIPropaty("Win32_ComputerSystem", "Model"));
            resultJson.Add("UserName", FromWMI.GetWMIPropaty("Win32_ComputerSystem", "UserName").ToString());

            resultJson.Add("OSType", FromWMI.GetWMIPropaty("Win32_OperatingSystem", "OSType").ToString());
            //OSPlatform
            //OSRelease
            resultJson.Add("OSname", (string)FromWMI.GetWMIPropaty("Win32_OperatingSystem", "Caption"));
            resultJson.Add("OSBuildNumber", (string)FromWMI.GetWMIPropaty("Win32_OperatingSystem", "BuildNumber"));
            resultJson.Add("OSVersion", (string)FromWMI.GetWMIPropaty("Win32_OperatingSystem", "Version"));
            //OSServicePack
            //Description
            resultJson.Add("OSInstallDate", FromWMI.GetWMIPropaty("Win32_OperatingSystem", "InstallDate").ToString());
            resultJson.Add("LastBootUpTime", (string)FromWMI.GetWMIPropaty("Win32_OperatingSystem", "LastBootUpTime"));
            resultJson.Add("OSRegisteredUser", (string)FromWMI.GetWMIPropaty("Win32_OperatingSystem", "RegisteredUser"));
            resultJson.Add("OSSerialNumber", FromWMI.GetWMIPropaty("Win32_OperatingSystem", "SerialNumber").ToString());

            var archStr = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITEW6432"); //現在のプロセスがWOW64で仮想化している場合は元のアーキテクチャを取得
            if (archStr == null)
            {
                archStr = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"); //仮想化されていない場合はプロセスが動作しているアーキテクチャを取得
            }
            resultJson.Add("OSarch", archStr);

            resultJson.Add("TotalMemory", FromWMI.GetWMIPropaty("Win32_OperatingSystem", "TotalVisibleMemorySize").ToString());

            //Dammy CPU DiscDriver Volume Group NTDomain FirewallProfiles Services Processes SharedResources MSPatch
            resultJson.Add("CPU", new JArray());
            resultJson.Add("Diskdrive", new JArray());
            resultJson.Add("Volume", new JArray());
            resultJson.Add("Group", new JArray());
            resultJson.Add("NTDomain", new JArray());
            resultJson.Add("FirewallProfiles", new JArray());
            resultJson.Add("Services", new JArray());
            resultJson.Add("SharedResources", new JArray());
            resultJson.Add("MSPatch", new JArray());
            resultJson.Add("Processes", new JArray());


            //LogoutForcedMinutes


            //ActiveDirectory からの取得
            using (DirectoryEntry rootDSE = GetDirectoryEntry("RootDSE"))
            {
                string domainRoot = rootDSE.Properties["defaultNamingContext"].Value as string;
                using (DirectoryEntry rootEntry = GetDirectoryEntry(domainRoot))
                {
                    string[] policyAttributes = new string[]
                    {
                        "maxPwdAge","minPwdAge","minPwdLength",
                        "lockoutDuration", "lockOutObservationWindow",
                        "lockoutThreshold", "pwdHistoryLength"
                    };

                    DirectorySearcher searcher = new DirectorySearcher(rootEntry, null, policyAttributes, SearchScope.Base);

                    SearchResult result = searcher.FindOne();

                    if (result != null)
                    {
                        if (result.Properties.Contains("minPwdAge")) //パスワードの変更禁止期間 PasswordMindays
                        {
                            resultJson.Add("PasswordMindays", new TimeSpan(Math.Abs((long)result.Properties["minPwdAge"][0])).TotalDays);
                        }
                        if (result.Properties.Contains("maxPwdAge")) //パスワードの有効期限 PasswordMaxdays
                        {
                            resultJson.Add("PasswordMaxdays", new TimeSpan(Math.Abs((long)result.Properties["maxPwdAge"][0])).TotalDays);
                        }
                        if (result.Properties.Contains("minPwdLength")) //パスワードの長さ PasswordLength
                        {
                            resultJson.Add("PasswordLength", result.Properties["minPwdLength"][0].ToString());
                        }
                        if (result.Properties.Contains("pwdHistoryLength")) //パスワードの履歴の記録
                        {
                            resultJson.Add("PasswordHistory", result.Properties["pwdHistoryLength"][0].ToString());
                        }
                        if (result.Properties.Contains("lockoutThreshold")) //アカウントロックアウトの閾値
                        {
                            resultJson.Add("LockoutRetries", result.Properties["lockoutThreshold"][0].ToString());
                        }
                        if (result.Properties.Contains("lockoutDuration")) //ロックアウト時間 LockingTime
                        {
                            long tick = (long)result.Properties["lockoutDuration"][0];
                            if (tick == long.MinValue)
                            {
                                tick = 0;
                            }
                            else
                            {
                                tick = Math.Abs(tick);
                            }
                            resultJson.Add("LockingTime", new TimeSpan(tick).TotalMinutes);
                        }
                        if (result.Properties.Contains("lockOutObservationWindow")) //ロックアウトカウンタのリセット LockCntReset
                        {
                            resultJson.Add("LockCntReset", new TimeSpan(Math.Abs((long)result.Properties["lockOutObservationWindow"][0])).TotalMinutes);
                        }

                    }
                }
            }

            //ComputerRole

            {//NTPServerAddr
                string rKeyName = @"SYSTEM\CurrentControlSet\Services\W32Time\Parameters";
                string rGetValueName = "NtpServer";
                try
                {
                    Microsoft.Win32.RegistryKey rKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(rKeyName);
                    resultJson.Add("NTPServerAddr", (string)rKey.GetValue(rGetValueName));
                    rKey.Close();
                }
                catch (NullReferenceException) { }
            }

            {//WsusServer
                string rKeyName = @"SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";
                string rGetValueName = "WUServer";
                try
                {
                    Microsoft.Win32.RegistryKey rKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(rKeyName);
                    resultJson.Add("WsusServer", (string)rKey.GetValue(rGetValueName));
                    rKey.Close();
                }
                catch (NullReferenceException) { }
            }



            {//[LocalAdmins]
                var LocalAdmins = new JArray();
                LocalAdmins.Add("Dammy");
                LocalAdmins.Add("Dammy2");
                resultJson.Add("LocalAdmins", LocalAdmins);
            }
            using (var user = UserPrincipal.Current) //CurrentUser
            {
                var currentUserJson = new JObject();
                //CurrentUserInfo/uid
                //CurrentUserInfo/gid
                //CurrentUserInfo/username
                currentUserJson.Add("username", user.UserPrincipalName);
                currentUserJson.Add("CurrentUserInfo/homedir", System.Environment.GetEnvironmentVariable("HOMEDRIVE") + System.Environment.GetEnvironmentVariable("HOMEPATH"));
                //CurrentUserInfo/shell
                currentUserJson.Add("CurrentUserInfo/LastLogonDate", user.LastLogon.ToString());
                //CurrentUserInfo/PasswordLastChange
                currentUserJson.Add("CurrentUserInfo/PasswordLastChange", user.LastPasswordSet.ToString());
                //CurrentUserInfo/CurrentLevel
                resultJson.Add("CurrentUserInfo", currentUserJson);
            }


            {
                var screenSaverJson = new JObject();

                screenSaverJson.Add("ScreenSaveActive", "Dammy");
                screenSaverJson.Add("ScreenSaverIsSecure", "Dammy");
                screenSaverJson.Add("ScreenSaveTimeOut", "Dammy");
                //ScreenSaver/ScreenSaver
                //ScreenSaver/ScreenSaveTimeOut
                //ScreenSaver/ScreenSaverIsSecure
                //ScreenSaver/ScreenSaveActive
                resultJson.Add("ScreenSaver", screenSaverJson);
            }

            {
                var fireWallJson = new JObject();
                fireWallJson.Add("BootTimeRuleCategory", "Dammy");
                fireWallJson.Add("FirewallRuleCategory", "Dammy");
                fireWallJson.Add("StealthRuleCategory", "Dammy");
                fireWallJson.Add("ConSecRuleRuleCategory", "Dammy");
                //FirewallConfig/BootTimeRuleCategory
                //FirewallConfig/FirewallRuleCategory
                //FirewallConfig/StealthRuleCategory
                //FirewallConfig/ConSecRuleRuleCategory
                resultJson.Add("FirewallConfig", fireWallJson);
            }

            {
                var modFileJson = new JObject();
                {//MODFileEncryption/InstalledDate
                    var directory = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments));
                    modFileJson.Add("InstalledDate", System.IO.File.GetCreationTime(directory.Parent.FullName + @"\MST\\FCS\LogPos.bin").ToString());
                }
                using (System.ServiceProcess.ServiceController scTemp = new System.ServiceProcess.ServiceController("svccli", "."))
                {//MODFileEncryption/State
                    modFileJson.Add("State", scTemp.Status.ToString());
                }
                resultJson.Add("MODFileEncryption", modFileJson);
            }

            var logsJson = new JObject();
            var logNames = new string[] { "Application", "System" }; //Logs
            foreach (string logName in logNames)
            {
                var logJson = new JObject();
                try
                {
                    using (System.Diagnostics.EventLog appLog = new System.Diagnostics.EventLog(logName))
                    {

                        logJson.Add("LogRetention", appLog.OverflowAction.ToString());
                        //DoNotOverwrite = -1,
                        //     「イベントを上書きしないでログをアーカイブする。」または「イベントは上書きしない。（ログは手動で消去）」
                        //OverwriteAsNeeded = 0,
                        //     「必要に応じてイベントを上書きする（もっとも古いイベントから）」
                        //OverwriteOlder = 1
                        //     保存日数よりも、新しいイベントがイベントを上書きすることを示す。、 System.Diagnostics.EventLog.MinimumRetentionDays
                        //logJson.Add("保存日数", appLog.MinimumRetentionDays);
                        //Logs/System/LogAutoBackup 
                        logJson.Add("LogMaxSize", appLog.MaximumKilobytes);
                    }

                    using (System.Diagnostics.Eventing.Reader.EventLogConfiguration appLogConf = new System.Diagnostics.Eventing.Reader.EventLogConfiguration(logName))
                    {
                        logJson.Add("LogFileName", appLogConf.LogFilePath);
                        using (System.Diagnostics.Eventing.Reader.EventLogSession appLogSes = new System.Diagnostics.Eventing.Reader.EventLogSession())
                        {
                            System.Diagnostics.Eventing.Reader.EventLogInformation appLogInfo = appLogSes.GetLogInformation(appLogConf.LogName, System.Diagnostics.Eventing.Reader.PathType.LogName);
                            logJson.Add("LogFileSize", appLogInfo.FileSize);
                            logJson.Add("LogCreationTime", appLogInfo.CreationTime);
                            logJson.Add("LogLastAccessTime", appLogInfo.LastAccessTime);
                            logJson.Add("LogLastWriteTime", appLogInfo.LastWriteTime);
                        }
                    }
                }
                catch (System.Exception)
                {

                }
                logsJson.Add(logName, logJson);
            }
            {//　SecurityLogは管理者である必要があるため後で検討
                var logJson = new JObject();
                logJson.Add("LogLastWriteTime", "Dammy");
                //Logs/Security/LogLastWriteTime
                //Logs/Security/LogRetention
                //Logs/Security/LogAutoBackup
                //Logs/Security/LogFilemax
                //Logs/Security/LogMaxSize
                //Logs/Security/LogFileSize
                //Logs/Security/LogCreationTime
                //Logs/Security/LogLastAccessTime
                //Logs/Security/LogLastWriteTime
                logsJson.Add("Security", logJson);
            }
            resultJson.Add("Logs",logsJson);

            {//LocalUsers
                string query = String.Format("Select * from Win32_UserAccount where domain='{0}'", Environment.MachineName);
                ManagementObjectSearcher mos = new ManagementObjectSearcher(query);

                var localUsersJson = new JObject();

                foreach(ManagementObject scn in mos.Get())
                {
                    var userJson = new JObject();
                    userJson.Add("AccountType",scn["AccountType"].ToString());
                    userJson.Add("Caption",scn["Caption"].ToString());
                    userJson.Add("Description",scn["Description"].ToString());
                    userJson.Add("Disabled",scn["Disabled"].ToString());
                    userJson.Add("Domain",scn["Domain"].ToString());
                    userJson.Add("FullName",scn["FullName"].ToString());
                    //userDict["InstallDate"] = scn["InstallDate"].ToString();
                    userJson.Add("LocalAccount",scn["LocalAccount"].ToString());
                    userJson.Add("Lockout",scn["Lockout"].ToString());
                    userJson.Add("PasswordChangeable",scn["PasswordChangeable"].ToString());
                    userJson.Add("PasswordExpires",scn["PasswordExpires"].ToString());
                    userJson.Add("PasswordRequired",scn["PasswordRequired"].ToString());
                    userJson.Add("SID",scn["SID"].ToString());
                    userJson.Add("SIDType",scn["SIDType"].ToString());
                    userJson.Add("Status",scn["Status"].ToString());

                    localUsersJson.Add(scn["Name"].ToString(), userJson);

                }
                resultJson.Add("LocalUsers", localUsersJson);

            }

            {//NetStat
                resultJson.Add("TCP", Netstat.GetTCP());
                resultJson.Add("TCPv6", Netstat.GetTCP6());
                resultJson.Add("UDP", Netstat.GetUDP());
                resultJson.Add("UDPv6", Netstat.GetUDP6());
            }

            {//NIC 

                var nicJson = new JObject();

                System.Net.NetworkInformation.NetworkInterface[] adapters = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

                foreach(System.Net.NetworkInformation.NetworkInterface adapter in adapters)
                {
                    var adapterJson = new JObject();


                    if (adapter.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up)
                    {
                        var aryIpJson = new JArray();

                        System.Net.NetworkInformation.IPInterfaceProperties ip_prop = adapter.GetIPProperties();

                        System.Net.NetworkInformation.UnicastIPAddressInformationCollection addrs = ip_prop.UnicastAddresses;


                        foreach (System.Net.NetworkInformation.UnicastIPAddressInformation addr in addrs)
                        {
                            var ipJson = new JObject();

                            ipJson.Add("address",addr.Address.ToString());
                            ipJson.Add("netmask",addr.IPv4Mask.ToString());
                            ipJson.Add("Family", addr.Address.AddressFamily.ToString());

                            aryIpJson.Add(ipJson);
                        }

                        nicJson.Add(adapter.Name.ToString(), aryIpJson);
                    }
                }

                resultJson.Add("NIC",nicJson);
            }
            

            {//software

                var arySoftwareJson = new JArray();

                string rKeyName = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
                try
                {
                    Microsoft.Win32.RegistryKey rKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(rKeyName);

                    string[] arySubKeyNames = rKey.GetSubKeyNames();

                    rKey.Close();

                    foreach (string subKeyName in arySubKeyNames)
                    {
                        var regJson = new JObject();

                        Microsoft.Win32.RegistryKey sKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(rKeyName + @"\" + subKeyName);

                        regJson.Add("keyName",(string)sKey.Name);
                        regJson.Add("Name", (string)sKey.GetValue("DisplayName"));
                        regJson.Add("InstallDate",(string)sKey.GetValue("InatallDate"));
                        regJson.Add("Vender", (string)sKey.GetValue("Publisher"));

                        arySoftwareJson.Add(regJson);

                        sKey.Close();

                    }

                }
                catch (NullReferenceException) { }

                resultJson.Add("Softwares", arySoftwareJson);
            }

            //SHA-1でハッシュ値
            
            byte[] byteValue = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(resultJson));
            var sha1 = System.Security.Cryptography.SHA1CryptoServiceProvider.Create();
            var sha1HashByte = sha1.ComputeHash(byteValue);
            var hashText = new StringBuilder();
            for (int tempNum = 0; tempNum < sha1HashByte.Length; tempNum++)
            {
                hashText.AppendFormat("{0:X2}", sha1HashByte[tempNum]);
            }

            string hashString = hashText.ToString();

            //JSONファイルに出力
            using (var writer = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + @"\" + resultJson["Station"] + "_" + resultJson["Unit"] + "_" + hashString.ToLower() + ".json"))
            {
                writer.Write(JsonConvert.SerializeObject(resultJson));
            }

        }
    }

    public static class DomainManager
    {
        static DomainManager()
        {
            Domain domain = null;
            DomainController domainController = null;
            try
            {
                domain = Domain.GetCurrentDomain();
                DomainName = domain.Name;
                domainController = domain.PdcRoleOwner;
                DomainControllerName = domainController.Name.Split('.')[0];
                ComputerName = System.Environment.MachineName;
            }
            finally
            {
                if (domain != null)
                    domain.Dispose();
                if (domainController != null)
                    domainController.Dispose();
            }
        }

        public static string DomainControllerName { get; private set; }
        public static string ComputerName { get; private set; }
        public static string DomainName { get; private set; }
        public static string DomainPath {
            get
            {
                bool bFirst = true;
                System.Text.StringBuilder sbReturn = new System.Text.StringBuilder(200);
                string[] strlstDc = DomainName.Split('.');
                foreach (string strDc in strlstDc)
                {
                    if (bFirst)
                    {
                        sbReturn.Append("DC=");
                        bFirst = false;
                    }
                    else
                    {
                        sbReturn.Append(",DC=");
                    }
                    sbReturn.Append(strDc);
                }
                return sbReturn.ToString();
            }
        }

        public static string RootPath
        {
            get
            {
                return string.Format("LDAP://{0}/{1})", DomainName, DomainPath);
            }
        }
    }

    public static class Netstat
    {
        enum TCP_TABLE_CLASS
        {
            TCP_TABLE_BASIC_LISTNER,
            TCP_TABLE_BASIC_CONNECTIONS,
            TCP_TABLE_BASIC_ALL,
            TCP_TABLE_OWNER_PID_LISTNER,
            TCP_TABLE_OWNER_PID_CONNECTIONS,
            TCP_TABLE_OWNER_PID_ALL,
            TCP_TABLE_OWNER_MODULE_LISTNER,
            TCP_TABLE_OWNER_MODULE_CONNECTIONS,
            TCP_TABLE_OWNER_MODULE_ALL
        };

        enum UDP_TABLE_CLASS
        {
            UDP_TABLE_BASIC,
            UDP_TABLE_OWNER_PID,
            UDP_TABLE_OWNER_MODULE
        };

        [DllImport("iphlpapi.dll")]
        extern static uint GetExtendedTcpTable(IntPtr pTcpTable, ref int pdwSize, bool bOeder, uint ulAf, TCP_TABLE_CLASS TableClass, int Reserved);
        [DllImport("iphlpapi.dll")]
        extern static uint GetExtendedUdpTable(IntPtr pUdpTable, ref int pdwSize, bool bOeder, uint ulAf, UDP_TABLE_CLASS TableClass, int Reserved);
        [DllImport("Ws2_32.dll")]
        extern static void inet_ntop(uint ulAf, byte[] pAddr, StringBuilder pStringBuf, int StringBufSize);

        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCPROW_OWNER_PID
        {
            public int State;
            public int LocalAddr;
            public int LocalPort;
            public int RemoteAddr;
            public int RemotePort;
            public int OwningPid;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_TCP6ROW_OWNER_PID
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] LocalAddr;
            public int LocalScopeId;
            public int LocalPort;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] RemoteAddr;
            public int RemoteScopeId;
            public int RemotePort;
            public int State;
            public int OwningPid;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_UDPROW_OWNER_PID
        {
            public int LocalAddr;
            public int LocalPort;
            public int OwningPid;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MIB_UDP6ROW_OWNER_PID
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] LocalAddr;
            public int LocalScopeId;
            public int LocalPort;
            public int OwningPid;
        }

        public static string[] StateStrings = { "", "CLOSED", "LISTNING", "SYN_SENT", "SYN_RCVD", "ESTABLISHED", "FIN_WAIT1", "FIN_WAIT2",
                                                "CLOSE_WAIT", "CLOSING", "LAST_ACK", "TIME_WAIT", "DELETE_TCB" };

        public static string ipstr(int addr)
        {
            var b = BitConverter.GetBytes(addr);
            return string.Format("{0}.{1}.{2}.{3}", b[0], b[1], b[2], b[3]);
        }

        public static string ipstr6(byte[] addr6)
        {// using System.Text;
            uint AF_INET6 = 23;
            StringBuilder buf = new StringBuilder(46);
            inet_ntop(AF_INET6, addr6, buf, buf.Capacity);
            return string.Format(buf.ToString());

        }

        public static int htons(int i)
        {
            var tmp = (((0x000000ff & i) << 8) + ((0x0000ff00 & i) >> 8)) + ((0x00ff0000 & i) << 8) + ((0xff000000 & i) >> 8);
            return (int)tmp;
        }


        public static JToken GetTCP()
        {
            var aryTcpJson = new JArray();

            //TCPコネクション一覧取得
            //Console.WriteLine("Protocol \t LocalAddr \t RemoteAddr \t Status \t PID:ProcessName");
            int size = 0;
            uint AF_INET = 2;   //IPv4

            GetExtendedTcpTable(IntPtr.Zero, ref size, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
            var p = Marshal.AllocHGlobal(size);
            if (GetExtendedTcpTable(p, ref size, true, AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0) == 0)
            {
                var num = Marshal.ReadInt32(p);
                var ptr = IntPtr.Add(p, 4);
                for (int i = 0; i < num; i++)
                {
                    var portJson = new JObject();
                    var o = (MIB_TCPROW_OWNER_PID)Marshal.PtrToStructure(ptr, typeof(MIB_TCPROW_OWNER_PID));
                    if (o.RemoteAddr == 0)
                    {
                        o.RemotePort = 0;
                    }
                    Process process = Process.GetProcessById(o.OwningPid);
                    //Console.WriteLine(string.Format("TCP \t {0}:{1} \t {2}:{3} \t {4} \t {5}:{6}", ipstr(o.LocalAddr), htons(o.LocalPort), ipstr(o.RemoteAddr), htons(o.RemotePort), StateStrings[o.State], o.OwningPid, process.ProcessName));
                    //MessageBox.Show(o.LocalAddr.ToString());

                    portJson.Add("LocalAddr",ipstr(o.LocalAddr));
                    portJson.Add("LocalPort", htons(o.LocalPort));
                    portJson.Add("RemoteAddrt",ipstr(o.RemoteAddr));
                    portJson.Add("RemotePort", htons(o.RemotePort));
                    portJson.Add("Status", StateStrings[o.State]);
                    portJson.Add("PID", o.OwningPid + ":" + process.ProcessName);

                    ptr = IntPtr.Add(ptr, Marshal.SizeOf(typeof(MIB_TCPROW_OWNER_PID)));

                    aryTcpJson.Add(portJson);

                }
                Marshal.FreeHGlobal(p);
            }

            return aryTcpJson;
        }

        public static JToken GetTCP6() {

            var aryTcpJson = new JArray();

            //TCPコネクション(IPv6)一覧取得
            //Console.WriteLine("Protocol \t LocalAddr \t RemoteAddr \t Status \t PID:ProcessName");
            int size = 0;
            uint AF_INET6 = 23;   //IPv6

            GetExtendedTcpTable(IntPtr.Zero, ref size, true, AF_INET6, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);
            var r = Marshal.AllocHGlobal(size);
            if (GetExtendedTcpTable(r, ref size, true, AF_INET6, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0) == 0)
            {
                var num = Marshal.ReadInt32(r);
                var ptr = IntPtr.Add(r, 4);
                for (int i = 0; i < num; i++)
                {
                    var portJson = new JObject();
                    var o = (MIB_TCP6ROW_OWNER_PID)Marshal.PtrToStructure(ptr, typeof(MIB_TCP6ROW_OWNER_PID));
                    if (BitConverter.ToInt32(o.RemoteAddr, 0) == 0)
                    {
                        o.RemotePort = 0;
                    }
                    Process process = Process.GetProcessById(o.OwningPid);
                    //Console.WriteLine(string.Format("TCP \t [{0}]:{1} \t [{2}]:{3} \t {4} \t {5}:{6}", ipstr6(o.LocalAddr), htons(o.LocalPort), ipstr6(o.RemoteAddr), htons(o.RemotePort), StateStrings[o.State], o.OwningPid, process.ProcessName));
                    portJson.Add("LocalAddr", ipstr6(o.LocalAddr));
                    portJson.Add("LocalPort", htons(o.LocalPort));
                    portJson.Add("RemoteAddrt", ipstr6(o.RemoteAddr));
                    portJson.Add("RemotePort", htons(o.RemotePort));
                    portJson.Add("Status", StateStrings[o.State]);
                    portJson.Add("PID", o.OwningPid + ":" + process.ProcessName);
                    ptr = IntPtr.Add(ptr, Marshal.SizeOf(typeof(MIB_TCP6ROW_OWNER_PID)));

                    aryTcpJson.Add(portJson);

                }
                Marshal.FreeHGlobal(r);
            }
            return aryTcpJson;
        }

        public static JToken GetUDP()
        {

            var aryUdpJson = new JArray();

            //UDPコネクション一覧取得
            //Console.WriteLine("Protocol \t LocalAddr \t PID:ProcessName");
            int size = 0;
            uint AF_INET = 2;   //IPv4

            GetExtendedUdpTable(IntPtr.Zero, ref size, true, AF_INET, UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID, 0);
            var q = Marshal.AllocHGlobal(size);
            if (GetExtendedUdpTable(q, ref size, true, AF_INET, UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID, 0) == 0)
            {
                var num = Marshal.ReadInt32(q);
                var ptr = IntPtr.Add(q, 4);
                for (int i = 0; i < num; i++)
                {
                    var portJson = new JObject();

                    var o = (MIB_UDPROW_OWNER_PID)Marshal.PtrToStructure(ptr, typeof(MIB_UDPROW_OWNER_PID));
                    Process process = Process.GetProcessById(o.OwningPid);
                    //Console.WriteLine(string.Format("UDP \t {0}:{1} \t {2}:{3}", ipstr(o.LocalAddr), htons(o.LocalPort), o.OwningPid, process.ProcessName));
                    portJson.Add("LocalAddr", ipstr(o.LocalAddr));
                    portJson.Add("LocalPort", htons(o.LocalPort));
                    portJson.Add("PID", o.OwningPid + ":" + process.ProcessName);
                    ptr = IntPtr.Add(ptr, Marshal.SizeOf(typeof(MIB_UDPROW_OWNER_PID)));

                    aryUdpJson.Add(portJson);

                }
                Marshal.FreeHGlobal(q);

            }

            return aryUdpJson;
        }

        public static JToken GetUDP6() {

            var aryUdpJson = new JArray();

            //UDPコネクション(IPv6)一覧取得
            //Console.WriteLine("Protocol \t LocalAddr \t PID:ProcessName");
            int size = 0;
            uint AF_INET6 = 23;   //IPv6

            GetExtendedUdpTable(IntPtr.Zero, ref size, true, AF_INET6, UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID, 0);
            var s = Marshal.AllocHGlobal(size);
            if (GetExtendedUdpTable(s, ref size, true, AF_INET6, UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID, 0) == 0)
            {
                var num = Marshal.ReadInt32(s);
                var ptr = IntPtr.Add(s, 4);
                for (int i = 0; i < num; i++)
                {
                    var portJson = new JObject();

                    var o = (MIB_UDP6ROW_OWNER_PID)Marshal.PtrToStructure(ptr, typeof(MIB_UDP6ROW_OWNER_PID));
                    Process process = Process.GetProcessById(o.OwningPid);
                    //Console.WriteLine(string.Format("UDP \t [{0}]:{1} \t {2}:{3}", ipstr6(o.LocalAddr), htons(o.LocalPort), o.OwningPid, process.ProcessName));
                    portJson.Add("LocalAddr", ipstr6(o.LocalAddr));
                    portJson.Add("LocalPort", htons(o.LocalPort));
                    portJson.Add("PID", o.OwningPid + ":" + process.ProcessName);
                    ptr = IntPtr.Add(ptr, Marshal.SizeOf(typeof(MIB_UDP6ROW_OWNER_PID)));

                    aryUdpJson.Add(portJson);

                }
                Marshal.FreeHGlobal(s);
            }
            return aryUdpJson;

        }

    }

    public class FromWMI
    {
 //       private const int INDENT_LETTERS = 4;

        // 端末の指定したWMIクラスのすべてのインスタンスを取得するメソッド
        public static ManagementObjectCollection GetWMICollection(string strWMIName)
        {
            using (ManagementClass wmiClass = new ManagementClass(strWMIName))
            {
                wmiClass.Get();
                wmiClass.Scope.Options.EnablePrivileges = true;
                return wmiClass.GetInstances();
            }
        }

        // 端末の指定したWMIクラスの指定したプロパティを取得する
        public static Object GetWMIPropaty(string strWMIName, string strPropatyName)
        {
            using (ManagementObjectCollection wmiObjectCollection = GetWMICollection(strWMIName))
            {
                var propatyObj = new Object();
                foreach (ManagementObject mngObj in wmiObjectCollection)
                {
                    propatyObj = mngObj[strPropatyName];
                }
                return propatyObj;
            }
        }

        // 端末の指定したWMIクラスのすべてのプロパティをJSON形式で取得する
        public static JProperty GetJProperties(string strWMIName)
        {
            var baseJArray = new JArray();
            var baseJObject = new JObject();
            try
            {

                using (ManagementObjectCollection wmiObjectCollection = GetWMICollection(strWMIName))
                {
                    foreach (ManagementObject mngObj in wmiObjectCollection)
                    {
                        var workingJObject = new JObject();
                        foreach (PropertyData propertyData in mngObj.Properties)
                        {
                            workingJObject.Add(new JProperty(propertyData.Name, propertyData.Value));
                        }

                        mngObj.Dispose();

                        if (wmiObjectCollection.Count > 1)
                        {
                            baseJArray.Add(workingJObject);
                        }
                        else
                        {
                            baseJObject = workingJObject;
                        }
                    }
                    if (wmiObjectCollection.Count > 1)
                    {
                        return new JProperty(strWMIName, baseJArray);
                    }
                    else
                    {
                        return new JProperty(strWMIName, baseJObject);
                    }
                }

            }
            catch (Exception)
            {
                return new JProperty(strWMIName, null);
                throw;
            }
        }

    }

    public class Crypto
    {
        private static readonly int INDENT_LETTERS = 4;
        private int indentCount = 0;

        private static string AddLine(string _baseStr, string _addingStr, int _indentCount = 0)
        {
            string _indentSpace = "";
            _indentCount = 0; //インデント無効化（最終的にはAddLineの削除）
            for (int i = 0; i < _indentCount; i++)
            {
                for (int j = 0; j < INDENT_LETTERS; j++)
                {
                    _indentSpace += " ";
                }
            }
            return _baseStr = (_baseStr + _indentSpace + _addingStr + "\r\n");
        }

        public string GetFormInputs(Dictionary<string, string> inputWordsDictionary)
        {
            string _output = "";
            foreach (var item in inputWordsDictionary)
            {
                _output = AddLine(_output, $"\"{item.Key}\":\"{item.Value}\",", indentCount);
            }
            return _output;
        }

        public string GetSha256Hash(string plain)
        {
            var sha256Provider = new SHA256CryptoServiceProvider();
            byte[] bArray = sha256Provider.ComputeHash(System.Text.Encoding.UTF8.GetBytes(plain));
            string hash = null;
            for (int i = 0; i < bArray.Length; i++)
            {
                hash += bArray[i].ToString("x2");
            }
            return hash;
        }

        public string EncryptoAesCipher(string plain, string keyIv)
        {
            byte[] encrypted;
            string _keyIv = keyIv;
            while (_keyIv.Length < 48)
            {
                _keyIv += "_" + keyIv;
            }

            using (var aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = System.Text.Encoding.UTF8.GetBytes(_keyIv.Substring(0, 32));
                aesProvider.IV = System.Text.Encoding.UTF8.GetBytes(_keyIv.Substring(32, 16));
                using (var encrypter = aesProvider.CreateEncryptor(aesProvider.Key, aesProvider.IV))
                {
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encrypter, CryptoStreamMode.Write))
                        {
                            using (var writer = new StreamWriter(csEncrypt))
                            {
                                writer.Write(plain);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }
            }
            return System.Convert.ToBase64String(encrypted, 0, encrypted.Length);
        }

        public string DecriptAesCipher(string cipher, string keyIv)
        {
            byte[] encrypted = System.Convert.FromBase64String(cipher);
            string _keyIv = keyIv;
            while (_keyIv.Length < 48)
            {
                _keyIv += "_" + keyIv;
            }

            using (var aesProvider = new AesCryptoServiceProvider())
            {
                aesProvider.Key = System.Text.Encoding.UTF8.GetBytes(_keyIv.Substring(0, 32));
                aesProvider.IV = System.Text.Encoding.UTF8.GetBytes(_keyIv.Substring(32, 16));
                using (var decrypter = aesProvider.CreateDecryptor(aesProvider.Key, aesProvider.IV))
                {
                    using (var msDecrypt = new MemoryStream(encrypted))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decrypter, CryptoStreamMode.Read))
                        {
                            using (var reader = new StreamReader(csDecrypt))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}
