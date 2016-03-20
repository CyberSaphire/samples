[CmdletBinding]
function Get-VsoRepository {
    [OutputType([String[]])]
    Param
    (
        [Parameter(Mandatory=$true)]
        $UserName,
        [Parameter(Mandatory=$true)]
        $Password,
        [Parameter(Mandatory=$true)]
        $VsoAccount,
        [Parameter(Mandatory=$true)]
        $ProjectName
    )

    $headers = @{Authorization=("Basic {0}" -f (Get-VsoBasicAuthHeader -UserName $UserName -Password $Password))}
    # alternative uri
    # $uri = ("https://{0}.visualstudio.com/defaultcollection/_apis/git/{1}/repositories" -f $VsoAccount, $ProjectId)
    $uri = ("https://{0}.visualstudio.com/defaultcollection/{1}/_apis/git/repositories?api-version=1.0" -f $VsoAccount, $ProjectName)

    Invoke-RestMethod `
     -Method Get `
    -Uri $uri `
    -headers $headers

}

[CmdletBinding]
function Get-VsoPullRequest {
    Param
    (
        [Parameter(Mandatory=$true)]
        $UserName,
        [Parameter(Mandatory=$true)]
        $Password,
        [Parameter(Mandatory=$true)]
        $VsoAccount,
        [Parameter(Mandatory=$true)]
        $ProjectName,
        [Parameter(Mandatory=$false)]
        [Hashtable]
        $RepoName
    )

    if($RepoName -eq $null)
    {

    }
    else
    {
        $repos = Get-VsoRepository -UserName $UserName -Password $Password -VsoAccount $VsoAccount -ProjectName $ProjectName

    }

}

[CmdletBinding]
function Get-VsoBasicAuthHeader
{
    [OutputType([String])]
    Param
    (
        $UserName,
        $Password
    )

    $basicAuth = ("{0}:{1}" -f $UserName,$Password)
    $basicAuth = [System.Text.Encoding]::UTF8.GetBytes($basicAuth)
    $basicAuth = [System.Convert]::ToBase64String($basicAuth)
    return $basicAuth
}

Export-ModuleMember Get-VsoRepository
Export-ModuleMember Get-VsoPullRequest