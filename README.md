# WorkTimeTracker

A simple, offline Android app built with .NET MAUI for tracking work hours. Designed for users with minimal digital experience, featuring a modern, clean, and intuitive interface.

## Features Implemented

- **Modern Main Page**  
  - Digital clock at the top using a rounded `Border`  
  - Large elapsed time counter in the center  
  - Single START/STOP button that changes color and text dynamically  
  - Timer logic integrated in the ViewModel  

- **Real-time updates**  
  - Date and time update every second  
  - Elapsed time updates every second while the timer is running  

- **MVVM Architecture**  
  - `MainPageViewModel` handles timer logic, date/time, and button state  
  - Bindings use `x:DataType` for compiled bindings and better performance  

## Technical Stack

- **.NET MAUI** (C#)  
- MVVM pattern with **CommunityToolkit.Mvvm**  
- Local SQLite database ready (not yet connected to UI)  
- Modern UI with `Border`, large typography, and stylized buttons  
- Fully offline, no authentication, no cloud  

## Project Structure

- `Views/` → MainPage UI  
- `ViewModels/` → MainPageViewModel  
- `Resources/` → Fonts, images, splash screens  
- `Platforms/` → Android-specific setup  

## Next Steps

- Implement **Work Types management page** (CRUD for types of work)  
- Implement **Settings / Summary page**  
- Connect the **SQLite database** for saving sessions  

## Getting Started

1. Clone the repository:
```bash
git clone https://github.com/TiagoMurtinho/WorkTimeTracker.git
```
2. Open in Visual Studio 2022+ with .NET MAUI workloads installed.
3. Build and run on an Android emulator or device.
