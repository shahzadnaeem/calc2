SRC=src
TESTS=tests

PROJECT=$(SRC)/Calc
LIB_PROJECT=$(SRC)/CalcLib
TEST_PROJECT=$(TESTS)/Calc.Test
TEST_RESULTS=$(TEST_PROJECT)/TestResults
TEST_REPORT=$(TEST_PROJECT)/TestReport

DOTNET=dotnet
COVERAGE_REPORTER=$(TEST_PROJECT)/scripts/createCoverageReport.sh

.PHONY: build clean watch run test coverage coverage-report coverage-report-html

build clean:
	$(DOTNET) $@

watch run:
	$(DOTNET) $@ --project $(PROJECT)

test:
	$(DOTNET) test $(TEST_PROJECT)

coverage:
	rm -rf $(TEST_RESULTS)
	$(DOTNET) test --collect "Xplat Code Coverage"

coverage-report: coverage
	$(COVERAGE_REPORTER) $(TEST_PROJECT)

coverage-report-html: coverage
	$(COVERAGE_REPORTER) $(TEST_PROJECT) HTML
