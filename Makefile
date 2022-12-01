SRC=src
TESTS=tests

PROJECT=$(SRC)/Calc
LIB_PROJECT=$(SRC)/CalcLib
TEST_PROJECT=$(TESTS)/Calc.Test

DOTNET=dotnet

.PHONY: build clean watch run test

build clean:
	$(DOTNET) $@

watch run:
	$(DOTNET) $@ --project $(PROJECT)

test:
	$(DOTNET) test $(TEST_PROJECT)
