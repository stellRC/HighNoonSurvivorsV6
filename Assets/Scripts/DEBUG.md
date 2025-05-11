## Git & Unity Integration

1. Make a new repository with a "Unity" .gitignore 
2. Rename or make a new unity project with SAME name as repository
3. Open terminal in root folder in VS code
4. *Don't panic* when this process takes awhile

- [Source: Unity Game Template](https://github.com/colinwilliams91/unity-game-template)
- [Source: Managing Remote Repositories](https://docs.github.com/en/get-started/git-basics/managing-remote-repositories)
```
# Initialize local repository
git init

# Set a new remote location with HTTPS
# use set-url instead of add if updating origin
git remote add origin https://gihub.com/your/repo.git

# Verify new remote
git remote -v
> origin  https://github.com/your/repo.git (fetch)
> origin  https://github.com/your/repo.git (push)

# Switch to main branch
git checkout main

# Rename branch temporarily
git branch -m main-temp

git fetch

git checkout main

# Create local main that matches remote main and sync
git pull origin main

# Merge local Unity project
git merge main-temp --allow-unrelated-histories
```

