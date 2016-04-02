<#
    Get-PsSysInfoRam
    .SYNOPSIS
        Returns RAM information on current computer.
    .DESCRIPTION
        This function uses the win32_PhysicalMemoryArray and win32_PhysicalMemory
        WMI Object to query RAM information on current computer.
    .PARAMETER
    .EXAMPLE
        C:\\\\PS>Get-PsSysInfoRam
        Total RAM slots available:      4
        Total RAM sticks installed:     2
        Maximum capacity allowed:       32GB
        Total RAM capacity installed:   8GB

        BankLabel   Capacity DeviceLocator  Caption         Manufacturer
        ---------   -------- -------------  -------         ------------
        BANK 1    4294967296 ChannelA-DIMM1 Physical Memory Samsung
        BANK 3    4294967296 ChannelB-DIMM1 Physical Memory Samsung
    .NOTES
    .LINK
#>
[CmdletBinding]
function Get-PsSysInfoRam {

    $ramArray = Get-WmiObject -Class "win32_PhysicalMemoryArray" -namespace "root\CIMV2"
    if($ramArray.Count -ne $null)
    {
        # more than one array?
        Write-Output "More than one win32_PhysicalMemoryArray objects found."
        return
    }

    $rams = Get-WmiObject -Class "win32_PhysicalMemory" -namespace "root\CIMV2"
    $totalRamInGB = 0
    $rams | % {$totalRamInGB = $totalRamInGB + ($_.Capacity/1GB)}

    Write-Output ("Total RAM slots available:`t{0}" -f $ramArray.MemoryDevices)
    Write-Output ("Total RAM sticks installed:`t{0}" -f $rams.Count)
    Write-Output ("Maximum capacity allowed:`t{0}" -f (`
        @{$false=$([int]($ramArray.MaxCapacity)/1024/1024).ToString() + "GB";`
            $true="Unknown"}[$ramArray.MaxCapacity -eq '0'])`
    ) # MaxCapacity is in KB
    Write-Output ("Total RAM capacity installed:`t{0}GB" -f $totalRamInGB)

    # alsternative to use following wmi
    # wmic MEMORYCHIP get BankLabel,Capacity,DeviceLocator,Caption,Manufacturer
    $rams | ft BankLabel,Capacity,DeviceLocator,Caption,Manufacturer | Write-Output

}

Export-ModuleMember Get-PsSysInfoRam