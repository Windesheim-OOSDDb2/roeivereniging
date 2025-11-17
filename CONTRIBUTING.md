# Contributing Guidelines

Thank you for your interest in contributing to **Roeivereniging**! This document describes how we organize work using *Gitflow*, how to set up your branches, submit contributions, and ensure code quality. Please follow these steps to help maintain clarity, consistency, and a stable development process.

---

## Gitflow Workflow

We use the standard Gitflow branching model to structure development. This helps us:

- keep the `master` branch always in a releasable/production-ready state  
- use `development` for integrating features before release  
- allow hotfixes/releases in a controlled way

Here are the branches we use:

| Branch | Purpose |
|--------|---------|
| `Master` | Production‑ready code. Merges only from `release/*` and `hotfix/*`. |
| `development` | Ongoing development. All feature branches are merged here. When ready, a release branch is cut from this. |
| `feature/<linear-identifier>` | New features, enhancements, or tasks (e.g. implementing UC05, adding new service, etc.). Branch off from `develop`. |
| `hotfix/<linear-identifier>` | For urgent fixes when the code in `Master` fails in production. Branch off `Master`. |
| `docs/<linear-identifier>` | Documentation updates. Branch off `development`. |
| `fix/<linear-identifier>` | For fixes when the code has failed. |
| `release/<version>` | Prepares a new production release. Branch off `development`. |

Check for more kinds of branches on [Conventional Branches](https://conventional-branch.github.io/)
### Branch Naming Conventions

- Feature branches: feature/<linear-identifier>  
  *Examples: feature/WIN-12 , feature/WIN-232
- Hotfix branches: hotfix/<linear-identifier>
  *Examples: hotfix/WIN-52

### Commit Message Conventions
Use a consistent style for commit messages to improve clarity. The format we use is:

```
<type>[optional scope]: <description>

[optional body]

```

Where `<type>` is one of:
- `feat` — a new feature
- `fix` — a bug fix
- `docs` — documentation only changes
- `style` — formatting, missing semicolons, etc; no code change
- `refactor` — code change that neither fixes a bug nor adds a feature
- `test` — adding missing tests or correcting existing tests
- `chore` — changes to the build process or auxiliary tools and libraries such as documentation
- `[optional scope]`this is used to tell in what project you are working. like `.Core` and `Core.data`
- `[optional body]` this is used for more detailed description of the type

*Examples:*
- `feat(Core.Data): implement GetAvailableProducts logic`
- `fix: uncomment login route in AppShell`

See [Conventional Commits](https://www.conventionalcommits.org/) for more details.

---

## Contribution Process

1. **Fork** the repository and clone to your local machine.  
   ```bash
   git clone <your‑fork‑url>
   cd roeivereniging
   git checkout develop
   ```

2. **Sync** your fork regularly with upstream (the original repo) to keep `development` up to date.

3. **Create a feature branch** off `development`:  
   ```bash
   git checkout -b feature/<linear-identifier> development
   ```

4. **Make your changes**, commit often with meaningful messages.

5. **Push your feature branch** to your fork:  
   ```bash
   git push origin feature/<linear-identifier>
   ```

6. **Open a Pull Request (PR)** from your feature branch → `development` (unless hotfix → `Master`). Provide a clear description of what you changed, referencing relevant UC (Use Case) if applicable. Request reviews.

7. **Code review & merge:**  
   - Make sure tests pass, code compiles, UI works.  
   - Fix review feedback.  
   - Squash or clean up minor commits in the PR so history is clear.  
   - When merging features, merge into `development`.  

---

## Release & Hotfix Process

- **Release Branch**  
  1. Once enough features are implemented and tested (e.g. for a sprint release), create `release/<version>` from `development`.  
  2. Update version number(s), update documentation, run final integration tests.  
  3. Merge release branch into `Master`.  
  4. Create release in GitHub and tag the release. 
  5. Make descriptive release notes.
  6. Publish the release.

- **Hotfix Branch**  
  1. If there is a critical bug in `Master`, branch: `hotfix/<linear-identifier>` from `Master`.  
  2. Fix, test.  
  3. Merge hotfix into `Master` and `development`.  
  4. Tag the fix on `Master`.  

---

## Additional Guidelines

- Always update **documentation** when needed.
- Ensure consistency in naming, file structure, and project layering (UI / Core / Data).
- Try to write or maintain tests if possible. Even basic ones help.
- Use descriptive commit messages; avoid “fixed stuff” or “changes” without context.
- Only the creator of a PR may merge the PR.
- **In the branch section, it will be feature/<identifier> or fix/<identifier>.** 
---

## Resources

- [Conventional Branches](https://conventional-branch.github.io/) 
- [Conventional Commits](https://www.conventionalcommits.org/)  
