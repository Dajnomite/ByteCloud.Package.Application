using ByteCloud.Package.Application;
using ByteCloud.Package.Logging;

string access = new EnvironmentVariable("VAULT_ACCESS").Value;

string key = "DB_PORT";
bool success = Secret.Write(access, key, "5433");
Log.Assert(success);

string result = Secret.Read(access, key);

Log.Info("Done " + result);