ParkEase – Smart Parking Reservation & Management System

**Overview**
ParkEase is a modern smart parking reservation and management mobile application developed using .NET MAUI, C#, SQLite, AI integration, analytics dashboards, and GPS navigation services. The application was designed to improve parking accessibility, simplify reservation workflows, and modernize parking management operations through intelligent mobile technologies.

The system allows users to locate parking facilities, reserve parking spaces, manage bookings and vehicles, simulate digital payments, receive QR-based confirmations, and navigate directly to parking locations using integrated GPS and Google Maps services.

ParkEase also includes administrator and operator management tools for parking lot management, occupancy tracking, operational analytics, AI-powered parking recommendations, and intelligent parking insights.


**Features**

**User Features**
* User Authentication & Registration
* Role-Based Access Control
* Smart Parking Reservations
* Vehicle Management
* Simulated Payment Processing
* QR Reservation Confirmation
* GPS Navigation & Google Maps Integration
* Nearby Parking Discovery
* AI Parking Assistant
* Animated Mobile Navigation
* Profile Image Upload

**Administrative Features**
* Parking Lot CRUD Management
* Occupancy Monitoring
* Revenue Tracking
* Booking Analytics
* Operational Dashboards
* Parking Availability Management
* Reservation Monitoring

**AI Features**
* OpenAI Integration
* Conversational Parking Assistant
* Intelligent Parking Recommendations
* Occupancy-Based Suggestions
* Cheapest Parking Recommendations
* Availability Analysis


**Technologies Used**
* .NET MAUI
* C#
* SQLite
* XAML
* CommunityToolkit.MVVM
* OpenAI API
* GPS Services
* Google Maps Integration

**System Architecture**
The application follows the MVVM (Model-View-ViewModel) architecture pattern and is organized into:
* Views
* ViewModels
* Models
* Services
* SQLite Database Layer

External integrations include:
* OpenAI API
* Google Maps
* GPS Services

**Core Modules**
* Login & Authentication
* Dashboard
* Parking Lots
* Booking Workflow
* Checkout & QR Confirmation
* Analytics Dashboard
* Admin Dashboard
* AI Assistant Widget
* GPS Navigation
* Profile Management

**Demo Accounts**

**Administrator**
Email:
admin@parkease.com
Password:
Admin123

Regular User
Email:
user@parkease.com

Password:
User123

How to Run the API
Open terminal inside:
ParkEase.Api

Run:

**powershell**
dotnet clean
dotnet build
dotnet run

Default API URL:
http://localhost:5201

**How to Run the Mobile App**

Open terminal inside:
ParkEase.Mobile

Run:
powershell
dotnet clean
dotnet build -f net10.0-windows10.0.19041.0
dotnet run -f net10.0-windows10.0.19041.0

**Database Structure**
Main database entities include:
* Users
* ParkingLots
* Vehicles
* Bookings

The database supports:
* Parking reservations
* Occupancy tracking
* Vehicle management
* User management
* Operational analytics

Analytics Features
The application includes:
* Revenue Tracking
* Booking Statistics
* Occupancy Monitoring
* Parking Utilization Charts
* AI-Style Insights
* Booking Status Breakdowns

Challenges Encountered
Several technical challenges were encountered during development, including:
* Occupancy synchronization
* Navigation crashes
* Database duplication handling
* GPS permissions
* AI integration
* MVVM binding issues

These were resolved through extensive debugging, testing, and architectural improvements.

Screenshots
The repository includes screenshots of:
* Login Screen
* Dashboard
* Parking Lots
* Booking Workflow
* Analytics Dashboard
* AI Assistant
* Admin Dashboard
* GPS Navigation
* QR Confirmation

**Academic Purpose**
This project was developed as part of an academic software engineering and mobile application development initiative to demonstrate:
* Cross-platform mobile development
* AI integration
* Database management
* Analytics visualization
* GPS navigation
* Smart system architecture
* Role-based administration

**Authors**
Developed by:
Joevan Duhaney
