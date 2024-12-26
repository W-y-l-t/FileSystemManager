# FileSystemManager
A console application for interacting with and managing the file system.

> The tests are written using the XUnit framework \
> CI/CD is also configured.

## Functionality

- **Navigation**: Navigating through the file system tree (relative and absolute paths).
- **Viewing**:
- Directory contents in the console with the depth parameter.
  - File contents.
- **File Management**:
- Move.
  - Copying.
  - Deletion.
  - Renaming.
- **Console management**:
- Connect/disconnect the file system.
  - Execution of commands using flags.

## Supported console commands

**CONNECTION COMMANDS** 
                                    
- connect [Address] [-m Mode] \
Address - Absolute path in the file system to connect to. \
Mode - File system mode (values: local, in-memory).

- disconnect \
Disconnects from the file system.

**FILE COMMANDS**

- file show [Path] {-m Mode} \
Path - Relative or absolute path to a file. \
Mode - File output mode (value: console).

- file move [SourcePath] [DestinationPath] \
SourcePath - Relative or absolute path to the file to be moved. \
DestinationPath - Relative or absolute path to the directory where the file should be moved.

- file copy [SourcePath] [DestinationPath] \
SourcePath - Relative or absolute path to the file to be copied. \
DestinationPath - Relative or absolute path to the directory where the file should be copied.

- file delete [Path] \
Path - Relative or absolute path to the file to be deleted.

- file rename [Path] [Name] \
Path - Relative or absolute path to the file to be renamed. \
Name - New name for the file.

**TREE COMMANDS**

- tree goto [Path] \
Path - Relative or absolute path to a directory in the file system.

- tree list {-d Depth} {-fm FileMark} {-dm DirectoryMark} {-o OutputMode} {-s Shift} \
Depth - A parameter defining the depth of the listing (default: 1). \
FileMark - A parameter defining the sequence of characters that will be used to designate files. \
DirectoryMark - A parameter defining the sequence of characters that will be used to designate folders. \
OutputMode - A parameter that determines where the file tree will be displayed (value: console, default: console). \
Shift - A parameter that defines the sequence of characters to indent (to show the hierarchy).

**SYSTEM COMMANDS**

- exit \
Exits the application.

- help {-o OutputMode} \
OutputMode - A parameter that determines where information be displayed (value: console, default: console). \
Shows supported commands.

- clear \
Clears the output in the console.

## Behavioral Patterns Used

- **Chain of responsibility**: for extensibility of the used set of commands
- **Command**: to process commands
- **Visitor**: to output the file system tree
- **Strategy**: for the possibility of using different file system implementations

## Available file systems

- **Local file system**: operations with it are implemented using the built-in .NET of mechanisms from System.IO
- **In-memory file system**: used for testing, the flexible structure is set by the user