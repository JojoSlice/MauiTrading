# MauiTrading

A cross-platform trading application built with .NET MAUI that enables users to trade stocks and assets with real-time data visualization and comprehensive trade management.

## Features

- **User Authentication** - Secure login and registration with JWT token-based authentication
- **Multi-Asset Trading** - Buy and sell stocks and various financial assets
- **Real-Time Charts** - Interactive candlestick charts powered by Syncfusion
- **Trade History** - Comprehensive trade history tracking and analysis
- **P&L Tracking** - Real-time profit and loss calculations
- **Season Management** - Organize trades into seasons for better tracking
- **Cross-Platform** - Runs on Android, iOS, macOS, and Windows

## Technology Stack

- **.NET 9.0** with .NET MAUI
- **MVVM Architecture** using CommunityToolkit.Mvvm
- **Syncfusion Charts** for data visualization
- **JWT Authentication** for secure user sessions
- **XAML** for UI design

## Platform Support

- Android 21.0+
- iOS 15.0+
- macOS Catalyst 15.0+
- Windows 10.0.17763.0+

## Installation

### Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Visual Studio 2022 (17.8+) with .NET MAUI workload
- For Android: Android SDK
- For iOS/macOS: Xcode and Apple Developer account
- For Windows: Windows 10 SDK

### Setup

1. Clone the repository:
```bash
git clone <repository-url>
cd mauitrading
```

2. Restore NuGet packages:
```bash
dotnet restore
```

3. Build the project:
```bash
dotnet build
```

4. Run on your desired platform:
```bash
# For Android
dotnet build -t:Run -f net9.0-android

# For iOS
dotnet build -t:Run -f net9.0-ios

# For Windows
dotnet build -t:Run -f net9.0-windows10.0.19041.0
```

## Project Structure

```
MauiTrading/
├── Models/              # Data models (Asset, Stock, TradeData, etc.)
├── Service/             # API and business logic services
├── View/                # XAML views/pages
├── ViewModel/           # ViewModels for MVVM pattern
├── Helpers/             # Utility classes
├── Resources/           # Images, fonts, and other assets
└── Platforms/           # Platform-specific code
```

## Key Components

### Models
- **Asset** - Represents tradable assets
- **Stock** - Stock-specific data
- **TradeData** - Individual trade information
- **Candle** - Candlestick chart data
- **PnL** - Profit and Loss calculations
- **Season** - Trading season management
- **User** - User account information

### Services
- **AuthService** - User authentication
- **RegistrationService** - New user registration
- **TradeService** - Opening new trades
- **CloseTradeService** - Closing existing trades
- **TradeHistoryService** - Historical trade data
- **AssetService** - Asset management
- **StocksService** - Stock data retrieval
- **CandleService** - Chart data
- **UserService** - User profile management
- **SeasonService** - Season management

### Views
- **MainPage** - Login screen
- **RegistrationPage** - User registration
- **HomePage** - Main dashboard
- **TradePage** - Trading interface with charts
- **HistoryPage** - Trade history viewer

## Dependencies

- **CommunityToolkit.Maui.Core** (11.1.0) - MAUI community extensions
- **CommunityToolkit.Mvvm** (8.4.0) - MVVM helpers and utilities
- **Syncfusion.Maui.Charts** (28.2.6) - Professional chart controls
- **System.IdentityModel.Tokens.Jwt** (8.5.0) - JWT token handling
- **Microsoft.Extensions.Logging.Debug** (9.0.0) - Debug logging

## Usage

1. **Register/Login** - Create an account or log in with existing credentials
2. **Browse Assets** - View available stocks and assets to trade
3. **Execute Trades** - Buy or sell assets with real-time pricing
4. **Monitor Charts** - Analyze market trends with candlestick charts
5. **Track History** - Review past trades and calculate P&L
6. **Manage Seasons** - Organize trades into seasons for better analysis

## Architecture

The application follows the **MVVM (Model-View-ViewModel)** pattern:
- **Models** define the data structure
- **Views** define the UI in XAML
- **ViewModels** handle business logic and data binding
- **Services** manage API calls and data operations

## License

[Add your license information here]

## Contributing

[Add contribution guidelines here]
