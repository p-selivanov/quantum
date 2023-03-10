Write-Host "Quantum UP"

$profiles = $args
if (!$profiles) { $profiles = @("infra") }

Write-Host "Profiles: $profiles"

$upCommand = "docker compose " +
"-f ../compose/infra.yaml " +
"-f ../compose/app.yaml " +
"--profile $($profiles -join " --profile ") " +
"--project-name quantum " +
"up -d"

iex $upCommand