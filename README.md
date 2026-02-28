# WorkTimeTracker

A simple, offline Android app built with .NET MAUI for tracking work hours. Designed for users with minimal digital experience, featuring a modern, clean, and intuitive interface.

## Features Implemented

- **Modern Main Page**  
  - Digital clock at the top using a rounded `Border`  
  - Large elapsed time counter in the center  
  - Single START/STOP button that changes color and text dynamically  
  - Timer logic integrated in the ViewModel  
  - On stopping, a modal allows selecting a **WorkType** and shows a summary of hours and earnings  
  - Work sessions are saved in the local SQLite database  

- **Work Types Management (Settings Page)**  
  - CRUD operations for work types: create, edit, delete  
  - Each WorkType includes a **name** and **hourly rate**  
  - Modal dialogs for creating/editing WorkTypes  
  - CollectionView shows all WorkTypes with edit/delete buttons  
  - Displays salary per hour in the list  

- **Management Page (Calendar)**  
  - Calendar view organized by month  
  - Defaults to the current month  
  - Displays all days in the month  
  - Days with work sessions are visually highlighted  
  - Selecting a day opens a popup summary showing hours worked and total earnings  
  - **Monthly summary button** shows a popup with all days’ totals and monthly total  
  - **Export options** from the monthly summary:  
    - Share summary via **WhatsApp**  
    - Export to **PDF**  
    - Export to **Excel**

- **Real-time updates**  
  - Date and time update every second  
  - Elapsed time updates every second while the timer is running  

- **MVVM Architecture**  
  - `MainPageViewModel` handles timer logic, date/time, and button state  
  - `SettingsPageViewModel` handles WorkTypes CRUD  
  - `ManagementPageViewModel` handles calendar days, highlighting, daily summaries, monthly summary, and export logic  
  - Bindings use `x:DataType` for compiled bindings and better performance  

## Technical Stack

- **.NET MAUI** (C#)  
- MVVM pattern with **CommunityToolkit.Mvvm**  
- Local SQLite database for WorkTypes and WorkSessions  
- Modern UI with `Border`, large typography, and stylized buttons  
- Fully offline, no authentication, no cloud  
- Export via **WhatsApp**, **PDF**, and **Excel**  

## Project Structure

- `Views/` → MainPage, SettingsPage, ManagementPage UI  
- `ViewModels/` → MainPageViewModel, SettingsPageViewModel, ManagementPageViewModel  
- `Models/` → WorkType, WorkSession, CalendarDay, MonthlyReport  
- `Resources/` → Fonts, images, splash screens  
- `Platforms/` → Android-specific setup  

## Next Steps

- Add detailed daily and monthly summaries in ManagementPage  
- Optional: sound or haptic feedback on timer events  
- Improve calendar UI with more styling and navigation between months  

## Getting Started

1. Clone the repository:
```bash
git clone https://github.com/TiagoMurtinho/WorkTimeTracker.git
```
2. Open in Visual Studio 2022+ with .NET MAUI workloads installed.
3. Build and run on an Android emulator or device.
