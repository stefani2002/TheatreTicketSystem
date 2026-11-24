# 🎭 МојТеатар (MojTeatar)

**A web application for managing theatre plays, showtimes, and ticket sales.**

Authors: Stefani Eftimovska (211062) and Marija Avramoska (211004)
Faculty of Computer Science and Engineering (FINKI), Skopje

> ⚠️ Payments run in **Stripe test mode** only. The app is developed for a local environment and is not production-ready without additional configuration.

---

## Overview

МојТеатар digitalizes the theatre ticket-buying experience. Visitors browse active plays, a monthly repertoire calendar, and play details. Logged-in users buy tickets online, and a PDF ticket is generated automatically after each successful payment. Admins manage plays, showtimes, and seat availability. A built-in AI assistant answers questions about the app using data from the database, replacing phone support at the box office.

## Features

**Guests**
- Browse plays (title, description, genre, duration)
- View the repertoire as a monthly calendar with dates and prices
- Chat with the AI assistant

**Registered users**
- Everything guests can do, plus:
- Select a showtime and ticket quantity (seat availability is checked)
- Pay by card via Stripe
- Receive an auto-generated PDF ticket with a unique code

**Admins**
- Everything users can do, plus:
- Full CRUD for plays and their showtimes
- Manage total and available seats per showtime

**AI assistant**
- Pop-up chat component powered by the OpenAI API (GPT-4o)
- Answers are grounded in application data from the database
- Shows a friendly error message ("AI service is unavailable") if the service cannot be reached

## Tech Stack

| Area | Technologies |
|------|--------------|
| Backend | ASP.NET Core MVC, C#, Entity Framework Core |
| Database | Microsoft SQL Server |
| Auth | ASP.NET Identity (roles: User / Admin) |
| Frontend | Razor Views, HTML5, Tailwind CSS, Bootstrap, JavaScript |
| Payments | Stripe.net (test environment) |
| PDF tickets | iText |
| AI | OpenAI API |
| IDE | Visual Studio 2026 |

## Architecture

The solution follows **Onion Architecture** with four layers:

| Layer | Responsibility |
|-------|----------------|
| **Domain** | Core entities (`Order`, `Play`, `Show`, `Ticket`, `User`), relations, business rules |
| **Repository** | Data access (`IPlayRepository`, `IShowRepository` and implementations) |
| **Service** | Business logic: plays/showtimes, ticket purchase, PDF generation, Stripe and OpenAI integration |
| **Web** | MVC controllers, views, and frontend |

Request flow: browser → controller → service → repository → database, and back.

## Database

| Entity | Key fields |
|--------|-----------|
| **Play** | Id, Title, Description, Genre, DurationMinutes, PosterImagePath |
| **Show** | Id, ShowTime, Price, TotalSeats, AvailableSeats, PlayId (FK) |
| **Ticket** | Id, SeatNumber, Price, Code, ShowId (FK), OrderId (FK), UserId |
| **Order** | Id, OrderDate, TotalPrice, UserId |
| **User** | ASP.NET Identity tables (Email, UserName, ...) |

A play has one or more shows; tickets belong to a show, an order, and a user.

## Getting Started

> Adjust names and paths to match your repository.

### Prerequisites

- Visual Studio 2022/2026 with the ASP.NET workload (or the .NET SDK)
- SQL Server (LocalDB or full instance)
- A [Stripe](https://dashboard.stripe.com/) account (test keys)
- An [OpenAI](https://openai.com/) API key

### Setup

```bash
git clone https://github.com/<your-username>/mojteatar.git
cd mojteatar
```

1. Set the connection string and API keys (use user secrets, not committed files):

```bash
cd <Web project folder>
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your connection string>"
dotnet user-secrets set "Stripe:SecretKey" "<sk_test_...>"
dotnet user-secrets set "Stripe:PublishableKey" "<pk_test_...>"
dotnet user-secrets set "OpenAI:ApiKey" "<your key>"
```

2. Apply migrations:

```bash
dotnet ef database update
```

3. Run the app:

```bash
dotnet run
```

On first run, seed logic creates one **admin** account. Check the seeding code for its credentials and change them before sharing the project.

### Test payments

Use Stripe's test card numbers (e.g. `4242 4242 4242 4242`, any future expiry, any CVC).

## Testing

Manual test scenarios cover:
- **Auth:** registration, login, redirect to login when buying without signing in, admin access
- **CRUD:** add/edit plays, showtimes visible to users, available seats decrease after purchase
- **Purchase:** quantity confirmation opens the payment form, PDF is generated after payment
- **AI assistant:** message gets a response; error shown when the service is down

## Limitations & Future Work

- Stripe is in test mode only; real transactions are not enabled
- AI assistant has a limited answer domain and uses a free GPT-4o tier
- Not deployed; scalability, security, and availability in production are not evaluated
- Planned: two-factor authentication and other advanced user-management features

## Acknowledgments

Stripe, OpenAI, iText, Tailwind CSS, diagrams.net, and TemplateMo for the design template.

## License

Add your license here (e.g. MIT).
