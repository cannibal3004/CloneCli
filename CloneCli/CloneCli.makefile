CSHARP_COMPILER = mcs
FILES = AssemblyInfo.cs ConsoleMenu.cs Main.cs Pi.GPIO.cs 
Debug_REFERENCES = -r:System.Core.dll
Release_REFERENCES = -r:System.Core.dll
Debug_FLAGS = -optimize- -define:DEBUG -debug+ -debug:full -nowarn:1701,1702 -filealign:512 -warn:4 -pkg:dotnet 
Release_FLAGS = -optimize- -nowarn:1701,1702 -filealign:512 -warn:4 -pkg:dotnet 
OUTPUT_FILE = CloneCli.exe
Debug_OUTPUT_FOLDER = bin/monoDebug
Release_OUTPUT_FOLDER = bin/monoRelease
TARGET = exe

# Builds all configurations for this project...
.PHONY: build_all_configurations
build_all_configurations: Debug Release 

# Builds the Debug configuration...
.PHONY: Debug
Debug: create_folders $(FILES)
	$(CSHARP_COMPILER) $(Debug_REFERENCES) $(Debug_FLAGS) -out:$(Debug_OUTPUT_FOLDER)/$(OUTPUT_FILE) -target:$(TARGET) $(FILES)

# Builds the Release configuration...
.PHONY: Release
Release: create_folders $(FILES)
	$(CSHARP_COMPILER) $(Release_REFERENCES) $(Release_FLAGS) -out:$(Release_OUTPUT_FOLDER)/$(OUTPUT_FILE) -target:$(TARGET) $(FILES)

# Creates the output folders for each configuration, and copies references...
.PHONY: create_folders
create_folders:
	mkdir -p $(Debug_OUTPUT_FOLDER)
	mkdir -p $(Release_OUTPUT_FOLDER)

# Cleans output files...
.PHONY: clean
clean:
	rm -f $(Debug_OUTPUT_FOLDER)/*.*
	rm -f $(Release_OUTPUT_FOLDER)/*.*

