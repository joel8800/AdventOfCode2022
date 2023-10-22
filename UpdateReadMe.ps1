# =====================================================================================================================
#
# PowerShell script to update README.md to display a table of puzzles and stars collected
# on AdventOfCode challenges
#
# This script uses the users session cookie to get the users main page.
# - JSON file with the current status is saved so we don't have to retrieve the same info daily
# - Gets the number of stars for each day from adventofcode.com
# - For each day with stars, it will go to that days puzzle to retrieve the title
# - A progress bar is generated using the number of stars out of 50
# - A table that displays the puzzles solved and number stars for each day
# 
# The user needs to get their unique session cookie from their browser and save it to the
# cookie file ($cookieFile).  It should be one line in the form "session=ba32a3....", it will be
# unique for each computer, so it should be ignored by git.
# 
# Set the $year variable to the current year.
#
# Edit the $header string to include any additional Markdown text to the section above the progress
# bar and table.
#
# The Solution Notes column is a place holder. The user can edit the DescText fields in the daily
# status JSON file with anything of interest for that puzzle.
# =====================================================================================================================

$year = 2022
$url = "https://adventofcode.com/$year"
$cookieFile = '.\aocCookie'

# Note:
# Solution Description is 'Add descriptive text here' by default
# Edit the .json file to change it. It should persist once set.

# This gets put at the top of the README.md
$header = @"
# Advent of Code $year
- This is the first year to do Advent of Code as intended. Daily. 
- Completed many of the puzzels on the day of release.
- Some puzzled did take several extra days to finish.
- Completed all of them by mid-January 2023

#### Progress:
<img style="display: block; margin-left: auto; margin-right: auto; width: 100%;"

"@

$pbFooter = @"

`talt=`"Progress Bar`">
</img>

"@

# =====================================================================================================================
# Functions 
# =====================================================================================================================
# Get the title of the specified day
function Get-DayTitle {
    param ($day)

    Write-Host "Requesting title for Day $day from $url"
    $dayUrl = "$url/day/$day"
    $dayResp = Invoke-WebRequest -Uri $dayUrl
    $content = $dayResp.Content 
    $content -Match '--- Day (.*) ---' | Out-Null
    $splits = $matches[1] -Split ': '
    $title = $splits[1]
    $title
}

# Generate README.md
function Write-ReadMeFile {
    $stars = Get-StarCount
    Write-Host "Total stars collected: $stars"
    Write-Host "---"
    Write-Host "Writing README.md file"
    $progressBar = "`tsrc=`"https://progress-bar.dev/$stars/?scale=50&title=StarsCollected&width=700&suffix=/50`""
    $readme = $header + $progressBar + $pbFooter + ($sortedDays | ConvertTo-MarkDownTable) 
    Set-Content -Path '.\README.md' -Value $readme
}

# Count the stars completed, one for each part of each day
function Get-StarCount {
    $stars = 0
    $sortedDays | ForEach-Object {
        if ($_.Part1) { $stars++ } 
        if ($_.Part2) { $stars++ }
    }
    return $stars
}

# Create markdown table of daily progress
function ConvertTo-MarkDownTable {
    [CmdletBinding()] param(
        [Parameter(Position = 0, ValueFromPipeLine = $True)] $InputObject
    )
    Begin {
        "| Day | Status | Source | Solution Notes |`r`n"
        "| - | - | - | - |`r`n"
    }
    Process {
        $dayLink = '[Day ' + ([string]$_.Day).PadLeft(2, '0') + ':  ' + $_.Title + '](' + $url + '/day/' + $_.Day + ')'
        $solLink = '[Solution](./Day' + ([string]$_.Day).PadLeft(2, '0') + '/Program.cs)'
        if ($_.Part1) { $pt1 = ':star:' } else { $pt1 = '' }
        if ($_.Part2) { $pt2 = ':star:' } else { $pt2 = '' }
        "| $dayLink | $pt1$pt2 | $solLink | " + $_.DescText + " |`r`n"
    }
    End {}
}

# =====================================================================================================================
# Script start
# =====================================================================================================================
Write-Host "Updating README.md for Advent of Code $year"

# Read saved status file
if (Test-Path -Path 'DayStatus.json') {
    Write-Host "Reading current completion status"
    $localStatus = (Get-Content "DayStatus.json" -raw) | ConvertFrom-Json
} 

# Read cookie file (ex. session="5423819...")
Write-Host "Reading session cookie file: $cookieFile"
$cookie = Get-Content -Path $cookieFile -TotalCount 1
$parts = $cookie -Split '='

# Make a session cookie object
$sessCookie = [System.Net.Cookie]::new()
$sessCookie.Name = $parts[0]
$sessCookie.Value = $parts[1]
$sessCookie.Domain = "adventofcode.com"

# Make a WebRequestSession object and add the cookie
$wrs = [Microsoft.PowerShell.Commands.WebRequestSession]::new()
$wrs.Cookies.Add($sessCookie)

# Get the main page
Write-Host "Requesting main page from $url"
$iwrResp = Invoke-WebRequest -Uri $url -WebSession $wrs
Write-Host "---"

# Grab all the links with aria-label, there should be up to 25 of them
$aLabel = 'aria-label'
$status = $iwrResp.Links | Select-Object -Property $aLabel | Where-Object -Property $aLabel

# Get day and star information from the main page
$dayList = [System.Collections.ArrayList]::new()
$status | ForEach-Object {
    # Get day number
    $_.($aLabel) -match '(\d+)' | Out-Null
    $day = [int]$Matches[0]

    # Get star status for part1 and part2
    # $part1 should always be true if $part2 is true
    $part2 = $_.$aLabel -match '(two)'
    if ($part2) {
        $part1 = $true
    }
    else {
        $part1 = $_.$aLabel -match '(one)'
    }
    
    # If any stars on this day, build a day object and add to list
    if (($part1 -or $part2) -eq $true) {

        Write-Host "Day $day status: part1 = $part1, part2 = $part2"

        # Get day info from local file and use its title
        if (Test-Path variable:localStatus) {
            $localDay = $localStatus | Where-Object -Property Day -eq $day
            if ([string]::IsNullOrEmpty($localDay.Title) -eq $false) {
                $title = $localDay.Title
            }
            else {
                $title = Get-DayTitle -Day $day
            }
            $descTxt = $localDay.DescText
        }
        else {
            $title = Get-DayTitle -Day $day
            $descTxt = 'Add descriptive text here'
        }

        # create object for the day and add to list
        $tmp = [PSCustomObject]@{
            Day      = $day
            Part1    = $part1
            Part2    = $part2
            Title    = $title
            Link     = '.\Day' + ([string]$day).PadLeft(2, '0') + '\Program.cs'
            DescText = $descTxt
        }
        $dayList.Add($tmp) | Out-Null
    }
}
# List may not be in order (AoC2015)
$sortedDays = $dayList | Sort-Object -Property Day

# Write the README.md file and save current status to json file
Write-ReadMeFile
$json = ConvertTo-Json -InputObject $sortedDays
$json | Out-File DayStatus.json

Write-Host "Done"
# script end
