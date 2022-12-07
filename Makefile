SRC=src
TESTS=tests

PROJECT=$(SRC)/Calc
LIB_PROJECT=$(SRC)/CalcLib
TEST_PROJECT=$(TESTS)/Calc.Test
TEST_RESULTS=$(TEST_PROJECT)/TestResults
TEST_REPORT=$(TEST_PROJECT)/TestReport

DOTNET=dotnet
COVERAGE_REPORTER=$(TEST_PROJECT)/scripts/createCoverageReport.sh

.PHONY: build clean distclean generated watch run test coverage coverage-report coverage-report-html

build:
	$(DOTNET) $@

clean:
	$(DOTNET) $@
	rm -rf $(TEST_RESULTS) $(TEST_REPORT)

distclean: clean
	rm -rf $(PROJECT)/obj ${PROJECT}/bin
	rm -rf $(LIB_PROJECT)/obj $(LIB_PROJECT)/bin
	rm -rf $(TEST_PROJECT)/obj $(TEST_PROJECT)/bin

# Show generated .NET build versions
generated:
	(find . -type d -a \( -name obj -o -name bin \) -exec ls -lR {} \;) | grep -E -e "net[0-9\.]+:"

watch run:
	$(DOTNET) $@ --project $(PROJECT)

test:
	$(DOTNET) test $(TEST_PROJECT)

# Coverage test run
coverage:
	rm -rf $(TEST_RESULTS)
	$(DOTNET) test $(TEST_PROJECT) --collect "Xplat Code Coverage"

# Coverage report (text)
coverage-report: coverage
	$(COVERAGE_REPORTER) $(TEST_PROJECT)

# Coverage report (HTML)
coverage-report-html: coverage
	$(COVERAGE_REPORTER) $(TEST_PROJECT) HTML
