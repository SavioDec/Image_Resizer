# 🖼️ Image Resizer Tool

A robust Command Line Interface (CLI) tool built with C# .NET 10 to automate batch image resizing. This project uses **ImageSharp** for efficient, cross-platform image processing.

## 🚀 Features

* **Proportional Resizing:** Define the target height, and the width is automatically calculated to maintain the original aspect ratio.
* **Automatic Organization:**
    * Monitors a specific **Input** folder.
    * Moves original files to a **Finished** folder.
    * Saves processed images to a **Resized** folder.
* **Smart Naming:** Adds a unique timestamp to processed files to prevent overwriting.
* **Cross-Platform:** Runs natively on Windows and Linux (including single-file executable support).

## 🛠️ Tech Stack

* [.NET 10](https://dotnet.microsoft.com/)
* [SixLabors.ImageSharp](https://docs.sixlabors.com/articles/imagesharp/index.html)

## 📂 Folder Structure

Upon execution, the tool automatically creates the following directory structure:

```
/ (Executable Root)
│
├── Input_Files/         <-- Drop your original images here
├── Resized_Files/       <-- Resized images will appear here
└── Finished_Files/      <-- Original images are moved here after processing
```

## ⚙️ How to Run (Development)

1.  **Clone the repository:**
    ```
    git clone [https://github.com/YOUR-USERNAME/YOUR-REPO.git](https://github.com/YOUR-USERNAME/YOUR-REPO.git)
    cd didaticos.redimensionador
    ```

2.  **Restore dependencies:**
    ```
    dotnet restore
    ```

3.  **Run the application:**
    ```
    dotnet run
    ```

## 📦 How to Build (Create Executable)

Use the following commands to generate a **single, self-contained executable** (no .NET installation required on the target machine).

### 🐧 For Linux (x64)
Generates a single binary file with native dependencies included.

```
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:DebugType=None
```
*Output location:* `bin/Release/net10.0/linux-x64/publish/`

### 🪟 For Windows (x64)
Generates a single `.exe` file.

```
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:DebugType=None
```
*Output location:* `bin/Release/net10.0/win-x64/publish/`

## 📝 Usage Example

1.  Run the executable.
2.  The program asks: `Enter the desired new height:`
3.  You type: `200` (and press Enter).
4.  Drop an image (e.g., `photo.jpg`) into the `Input_Files` folder.
5.  The tool automatically:
    * Creates a resized version (e.g., `photo_20260118_143000.jpg`) in `Resized_Files`.
    * Moves the original `photo.jpg` to `Finished_Files`.

---

## 📄 License

This project is for educational purposes and is licensed under the MIT License.
