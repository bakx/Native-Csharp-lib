#ifdef _WIN64
#include <windows.h>
#include <direct.h> // _getcwd
#define symLoad GetProcAddress
#else
#include dlfcn.h
#include unistd.h
#define symLoad dlsym
#endif

typedef void (cLogMessageInit)(void);
typedef void (cLogMessageInitConfig)(char*);
typedef void (cLogMessage)(int, char*);
cLogMessageInit* logInit;
cLogMessageInitConfig* logInitConfig;
cLogMessage* logMessage;

void cLogInit();
void cLogInitConfig();

int main(int argc, char argv[])
{
	cLogInitConfig();

	logMessage(0, "Verbose message");
	logMessage(1, "Debug message");
	logMessage(2, "Informational message");
	logMessage(3, "Warning message");
	logMessage(4, "Error message");
	logMessage(5, "Fatal message");
}

void cLogInit()
{
	#ifdef _WIN64
		HINSTANCE logCHandle = LoadLibraryA("SampleLog.dll");
	#else
		void logCHandle = dlopen("SampleLog.so", RTLD_LAZY);
	#endif

	logInit = (cLogMessageInit*) symLoad(logCHandle, "init");
	logMessage = (cLogMessage*) symLoad(logCHandle, "log_message");

	logInit(); // Initialize the log method in C#
}

void cLogInitConfig()
{
	#ifdef _WIN64
		HINSTANCE logCHandle = LoadLibraryA("SampleLog.dll");
	#else
		void logCHandle = dlopen("SampleLog.so", RTLD_LAZY);
	#endif

	logInitConfig = (cLogMessageInitConfig*) symLoad(logCHandle, "init_config");
	logMessage = (cLogMessage*) symLoad(logCHandle, "log_message");

	char* path;
	
	if ( (path = _getcwd( NULL, 0 )) == NULL )
		perror( "_getcwd error" );
	else
	{
		logInitConfig(path); // Initialize the log method in C#
		free(path);
	}
} 