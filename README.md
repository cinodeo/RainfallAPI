# Rainfall API

This project is an ASP.NET Core Web API for fetching rainfall readings from a remote API and exposing them through a RESTful interface.

## Description

The Rainfall API interacts with the Environment Agency's flood monitoring API to retrieve rainfall readings for specific stations. It provides endpoints to fetch rainfall readings based on station ID and allows specifying the count of readings to fetch.

## Getting Started

To get started with the Rainfall API, follow these steps:

1. Clone the repository to your local machine:

   ```bash
   git clone https://github.com/your-username/rainfall-api.git

## API Endpoints

      GET /api/Rainfall/id/{stationId}/readings?count={count}

Fetches rainfall readings for the specified station ID. Optionally, you can specify the number of readings to fetch using the count query parameter.

   - stationId: The ID of the station for which to fetch rainfall readings.
   - count (optional): The number of readings to fetch. Default is 10.

Example:

      GET /api/Rainfall/id/3680/readings?count=20

## Technologies Used
- ASP.NET Core
- C#
- HttpClient
- Newtonsoft.Json
- NUnit (for unit testing)
- Swagger (for API documentation)
