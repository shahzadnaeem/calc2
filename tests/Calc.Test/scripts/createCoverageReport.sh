#!/bin/bash

# From https://betterdev.blog/minimal-safe-bash-script-template/

set -o nounset  # Exit with an error when undefined variable is used
set -o errexit  # Exit with an error if any command fails
set -o errtrace # Propagate traps to functions and scripts

# Handle these error conditions in the cleanup function - see below
trap cleanup SIGINT SIGTERM ERR EXIT

# OPTIONAL:
## set -o pipefail # Provide pipe failure error status
## shopt -s extglob # Extended file globbing

script_dir=$(realpath "$(dirname "${BASH_SOURCE[0]}")")

# =============================================================================

cleanup() {
    trap - SIGINT SIGTERM ERR EXIT
}

dirMustExist() {
    DIR=$1

    if [ ! -d "$DIR" ]; then
        echo "ERROR: Directory not found - '${TESTS_DIR}'"
        exit 1
    fi
}

# =============================================================================

TESTS_DIR=${1:-NO_TEST_DIR_GIVEN}
POPUP_BROWSER=${2:-}

RESULTS="${TESTS_DIR}/TestResults"
REPORT="${TESTS_DIR}/TestReport"
REPORT_HTML_INDEX="${REPORT}/index.html"
REPORT_TEXT_SUMMARY="${REPORT}/Summary.txt"

dirMustExist "${TESTS_DIR}"
dirMustExist "${RESULTS}"

# Pick latest report - should only be one!
LATEST_RESULT=$(ls -tr ${RESULTS} | tail -1)
LATEST_RESULT="${RESULTS}/${LATEST_RESULT}/coverage.cobertura.xml"

# Remove existing report
rm -rf "${REPORT}"

reportgenerator -reports:"${LATEST_RESULT}" -targetdir:"${REPORT}" -reporttypes:"Html;TextSummary"

if [ "${POPUP_BROWSER}" != "" ]; then
    firefox "${REPORT_HTML_INDEX}"
else
    cat "${REPORT_TEXT_SUMMARY}"
fi
