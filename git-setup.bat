@echo off
echo Starting Git Setup...

:: Step 1: Initialize Git
git init

:: Step 2: Add .gitignore
echo .vs/ > .gitignore
echo bin/ >> .gitignore
echo obj/ >> .gitignore
echo *.user >> .gitignore
echo *.suo >> .gitignore
echo *.log >> .gitignore
echo *.tmp >> .gitignore

:: Step 3: Add files (except ignored ones)
git add .

:: Step 4: Commit
git commit -m "Initial commit"

:: Step 5: Add remote (Replace the below link with your actual GitHub repo URL)
git remote add origin https://github.com/YourUsername/YourRepoName.git

:: Step 6: Rename to main
git branch -M main

:: Step 7: Push to GitHub
git push -u origin main

echo Git Setup Done!
pause