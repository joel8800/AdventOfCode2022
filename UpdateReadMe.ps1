$year = 2022
$url = "https://adventofcode.com/$year"
$cookieFile = '.\aocCookie'

# Note:
# Solution Description is 'Add descriptive text here' by default
# Edit the .json file to change it. It should persist once set.

# This gets put at the top of the README.md
$header = @"
# Advent of Code $year
- Completed many of the puzzles on the day of release
- Some puzzles to several extra days to finish
- Completed all of them by mid-January 2023

"@

# =====================================================================================================================
# Functions 
# =====================================================================================================================
# Get the title of the specified day
function Get-DayTitle {
    param ($day)

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
    $progressBar = "#### Progress:  ![Progress](https://progress-bar.dev/$stars/?scale=50&title=StarsCollected&width=480&suffix=/50)`r`n"

    $readme = $header + $progressBar + ($sortedDays | ConvertTo-MarkDownTable) 
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
# Read saved status file
if (Test-Path -Path 'dayStatus.json') {
    $localStatus = (Get-Content "DayStatus.json" -raw) | ConvertFrom-Json
} 

# Read cookie file (ex. session="5423819...")
$cookie = Get-Content -Path $cookieFile -TotalCount 1
$parts = $cookie -Split '='

# Make a session cookie object
#$sessCookie = New-Object System.Net.Cookie
$sessCookie = [System.Net.Cookie]::new()
$sessCookie.Name = $parts[0]
$sessCookie.Value = $parts[1]
$sessCookie.Domain = "adventofcode.com"

# Make a WebRequestSession object and add the cookie
$wrs = [Microsoft.PowerShell.Commands.WebRequestSession]::new()
$wrs.Cookies.Add($sessCookie)

# Get the main page
$iwrResp = Invoke-WebRequest -Uri $url -WebSession $wrs

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

# script end
