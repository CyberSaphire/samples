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
        https://gallery.technet.microsoft.com/scriptcenter/PowerShell-script-to-list-82f88d0e
        http://www.nextofwindows.com/getting-ram-info-on-local-or-remote-computer-in-powershell
        http://www.kongsli.net/2012/02/23/powershell-another-alternative-to-ternary-operator/

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

    $ftExp =`
            @{Expression={$_.BankLabel};Label="BankLabel"},`
            @{Expression={$($_.Capacity/1GB).ToString() + "GB"};Label="Capacity"},`
            @{Expression={`
                    switch($_.MemoryType){`
                        2{"DRAM"}`
                        11{"Flash"}`
                        20{"DDR"}`
                        21{"DDR2"}`
                        24{"DDR3"}`
                        default{"Unknown"}}`
                    };`
                Label="MemoryType"},`
            @{Expression={$_.Speed};Label="Speed"},`
            @{Expression={$_.DeviceLocator};Label="DeviceLocator"},`
            @{Expression={$_.Caption};Label="Caption"},`
            @{Expression={$_.Manufacturer};Label="Manufacturer"}

    # alsternative to use following wmi
    # wmic MEMORYCHIP get BankLabel,Capacity,Speed,DeviceLocator,Caption,Manufacturer
    $rams | ft $ftExp | Write-Output

}

Export-ModuleMember Get-PsSysInfoRam