Write-Host "=== API ENDPOINTS TEST ===" -ForegroundColor Green

Write-Host "1. Swagger Status:" -ForegroundColor Cyan
try { 
  $r = Invoke-WebRequest http://localhost:5000/swagger -UseBasicParsing
  Write-Host "Status: $($r.StatusCode)" 
} catch { Write-Host "Failed: $_" }

Write-Host "`n2. Students List (page=1, pageSize=2):" -ForegroundColor Cyan
try { 
  $r = Invoke-WebRequest "http://localhost:5000/api/students?page=1&pageSize=2" -UseBasicParsing
  $j = $r.Content | ConvertFrom-Json
  Write-Host "Success: $($j.success)"
  Write-Host "Students: $($j.data.data.Count)"
  Write-Host "Total: $($j.data.pagination.totalItems)"
} catch { Write-Host "Failed: $_" }

Write-Host "`n3. Student by ID (1):" -ForegroundColor Cyan
try { 
  $r = Invoke-WebRequest "http://localhost:5000/api/students/1" -UseBasicParsing
  $j = $r.Content | ConvertFrom-Json
  Write-Host "Success: $($j.success)"
  Write-Host "ID: $($j.data.studentId), Name: $($j.data.fullName)"
} catch { Write-Host "Failed: $_" }

Write-Host "`n4. Courses List (page=1, pageSize=1):" -ForegroundColor Cyan
try { 
  $r = Invoke-WebRequest "http://localhost:5000/api/courses?page=1&pageSize=1" -UseBasicParsing
  $j = $r.Content | ConvertFrom-Json
  Write-Host "Success: $($j.success)"
  Write-Host "Courses: $($j.data.data.Count)"
} catch { Write-Host "Failed: $_" }

Write-Host "`n5. Enrollments List (page=1, pageSize=1):" -ForegroundColor Cyan
try { 
  $r = Invoke-WebRequest "http://localhost:5000/api/enrollments?page=1&pageSize=1" -UseBasicParsing
  $j = $r.Content | ConvertFrom-Json
  Write-Host "Success: $($j.success)"
  Write-Host "Enrollments: $($j.data.data.Count)"
} catch { Write-Host "Failed: $_" }
