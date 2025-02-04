# ProcessLogger

This project is a C# tool that logs the current processes running on the system, total CPU usage, and the CPU usage of the top 5 processes consuming the most CPU resources. The information is updated three times per second and saved to a JSON file.

## Features

- Logs current processes and their CPU usage.
- Displays total CPU usage.
- Identifies and logs the top 5 processes by CPU consumption.
- Updates the log every 1/3 of a second.
- Outputs data in JSON format for easy consumption.

## Getting Started

### Prerequisites

- .NET SDK (version 6.0 or later)

### Building the Project

1. Clone the repository:
   ```
   git clone <repository-url>
   ```
2. Navigate to the project directory:
   ```
   cd ProcessLogger
   ```
3. Restore the dependencies:
   ```
   dotnet restore
   ```
4. Build the project:
   ```
   dotnet build
   ```

### Running the Tool

To run the tool, execute the following command in the project directory:
```
dotnet run --project src/ProcessLogger.csproj
```

The tool will start logging the process information and save it to a JSON file in the output directory.

## Output

The output will be saved in a JSON file format, which can be easily parsed or analyzed by other tools or scripts.

## License

This project is licensed under the MIT License. See the LICENSE file for more details.