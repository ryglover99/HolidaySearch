# HolidaySearch

A simple holiday search library that finds the best value flight and hotel package based on departure airports, destination, date, and duration of stay.

# Features

Load flight and hotel data from JSON files

Search for holidays by departing from, traveling to, departure date, and duration

Score holidays by flight departure date closeness, hotel arrival date closeness, and total package price

Results are ordered by best value score, with the best value package being the first in the list

# Project Structure
```
HolidaySearch/
  Data/ JSON data for flights and hotels
  Models/ Core models (Flight, Hotel, HolidayPackage, HolidaySearchRequest, HolidaySearchResponse)
  Services/ HolidaySearchService and JsonDataLoaderService, with their respected interfaces
HolidaySearch.Tests/
  Data/ Test JSON data for flights and hotels, along with JsonDataLoader unit tests (xUnit) for flights and hotels
  Services/ HolidaySearchServiceTests, unit tests (xUnit) to test the HolidaySearchService
```
  

# Future Improvements

Add exception handling for missing or corrupt JSON data, at the moment exceptions will bubble up. In a production version errors would be handled

Add more detailed tests, along with more edge case tests

Support cancellation tokens for async calls

Add logging for debugging and monitoring

Improve scoring system by scoring on more factors such as duration of stay

Use DTO's and mapping to filter out Id when sending response to client
