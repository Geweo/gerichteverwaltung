# Testing Quality Rules

## Testing Framework

- Use xunit for all tests
- Use NSubstitute for mocking
- Do not emit "Act", "Arrange" or "Assert" comments
- Copy existing style in nearby files for test method names and capitalization

## Test Coverage

- Always include test cases for critical paths
- Test use cases thoroughly
- Mock ports when testing use cases
- Test adapters with integration tests where appropriate

