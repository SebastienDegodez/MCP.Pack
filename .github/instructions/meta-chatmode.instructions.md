---
applyTo: '.github/chatmodes/*.chatmode.md'
description: Template and requirements for writing meta-chatmode definitions for Copilot. Every chatmode file must include a description in its front-matter, and follow the documented structure and best practices.
---

# Meta-Chatmode Template

## Purpose
Describe the intent and scope of this chatmode meta-prompt. Explain what kind of chatmodes it is meant to generate or validate (e.g., assistant personas, interaction styles, specialized workflows).

## Persona or Role Description
Describe the intended persona, role, or behavior for this chatmode (e.g., "You are a helpful, encouraging code reviewer specializing in C#.").

## File Naming and Title Conventions
- Every meta-chatmode file must begin with a clear, descriptive title as the first heading (e.g., "Meta-Chatmode Template").
- The title must use sentence case and accurately reflect the file's purpose (e.g., "Meta-Chatmode Template").
- The filename must be clear, descriptive, in English, use hyphens, and end with the `.chatmode.md` extension.
- The file must be placed in the `.github/chatmodes/` directory at the root of the repository.
- The title and filename should be consistent with each other and with the file's content.

## Structure
- **Front-matter**: YAML block with at least `description` (short summary of the chatmode's intent). Optionally, a `tools` field can be included to specify which tools are allowed in this chatmode.
- **Sections**: Each chatmode file should include the following sections:
  - Title
  - Purpose
  - Persona or Role Description
  - Interaction Guidelines
  - Best Practices
  - Example Dialogues or Scenarios
  - References (optional)

## Rules
- Chatmode definitions must be clear, actionable, and unambiguous.
- Use imperative language for guidelines ("Respond as", "Avoid", "Always").
- Specify the intended persona, tone, and interaction style.
- If the chatmode is technology- or domain-specific, state it explicitly in the front-matter and body.
- Every chatmode front-matter **must** include a `description` field (short summary).
- The `tools` field (optional) can be used to restrict or allow specific tools for this chatmode (e.g., `tools: ["terminal", "notebook"]`).

## Best Practices
- Keep chatmode definitions concise but comprehensive.
- Use lists and example dialogues for clarity.
- Provide rationale for guidelines when relevant.
- Update chatmode definitions as needs evolve.

## Example
---
description: Friendly assistant for C# code reviews
tools: ['terminal', 'notebook']
---
# C# Code Review Mode Instructions
You are a helpful, encouraging code reviewer specializing in C#. Your task is to review C# code, provide constructive feedback, and suggest improvements.

## Instructions
Always:
* Provide actionable, specific suggestions.
* Use positive, supportive language.
* Reference C# best practices and documentation when relevant.
* Avoid harsh criticism; focus on improvement and learning.

## Example interaction
User: Can you review this method?
Assistant: Sure! Here are a few suggestions to improve readability and performance...

## References
- [C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)
