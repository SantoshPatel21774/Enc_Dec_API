
# 🔐 AES-256 Encryption & Decryption API – ASP.NET Core

## 📘 Description
This ASP.NET Core Web API project provides secure endpoints for AES-256 encryption and decryption. It is designed to help developers protect sensitive data using strong symmetric cryptography. The API is lightweight, configurable, and easy to integrate into other applications or services.

---

## 🎯 Purpose
The primary goal of this project is to:
- Provide a secure and reusable AES-256 encryption/decryption service.
- Enable secure data transmission and storage in enterprise applications.
- Demonstrate best practices for cryptographic operations in ASP.NET Core.

---

## 🚀 Features
- AES-256 encryption using a secure key and IV
- RESTful API endpoints for:
  - `/api/encrypt` – Encrypt plaintext
  - `/api/decrypt` – Decrypt ciphertext
- Configurable key and IV via app settings
- Input validation and error handling
- Swagger (OpenAPI) support for testing

---

## 🛠️ Technologies Used
- ASP.NET Core 9
- C#
- AES-256 (Advanced Encryption Standard)
- Swagger / OpenAPI
- Dependency Injection
- Configuration via `appsettings.json`

---

## 📂 Project Structure
```
Enc_Dec_API/
├── BAL/                     # Business logic layer
├── Common/                  # Shared utilities and constants
├── Core/                    # Core domain models and interfaces
├── DAL/                     # Data access layer
├── EncryptDecryptAPI/       # Main ASP.NET Core API project
│   ├── Controllers/         # API endpoints (Encrypt/Decrypt)
│   ├── Filters/             # Custom filters for validation/logging
│   ├── .env                 # Environment variables (AES key/IV)
│   ├── appsettings.json     # Configuration settings
│   ├── EncryptDecryptAPI.http # API testing file
│   ├── Program.cs           # Application entry point
│   ├── Startup.cs           # Service and middleware configuration
├── Logging/                 # Logging utilities
```

---

## 📡 API Endpoints

### 🔒 `/api/encrypt`
- **Method**: `POST`
- **Request Body**:
```json
{
  "RequestValue": "string"
}
```
- **Response**:
```json
{
  "IsScuccess": true,
  "Data": {
    "EncryptedValue": "QkjpiK7kTvPMOX4AeCENDQ=="
  },
  "Message": "Encryption completed",
  "ResponseTime": "2025-11-03T13:38:13.4558562+05:30"
}
```

### 🔓 `/api/decrypt`
- **Method**: `POST`
- **Request Body**:
```json
{
  "EncryptedValue": "QkjpiK7kTvPMOX4AeCENDQ=="
}
```
- **Response**:
```json
{
  "IsScuccess": true,
  "Data": {
    "DecryptedValue": "Santosh"
  },
  "Message": "Decryption completed",
  "ResponseTime": "2025-11-03T13:38:51.9069261+05:30"
}
```

---

## 🔧 Configuration
Set your AES key and IV in `appsettings.json`:
```json
"EncryptionKeyandIV": {
  "Key": "your-256-bit-base64-key",
  "IV": "your-128-bit-base64-iv"
}
```
> 🔐 **Note**: Use a secure method to generate and store your key and IV. Never hard-code secrets in production.

---

## 🧪 Testing
- Run the project using `dotnet run`
- Open your browser and navigate to `/swagger`
- Use Swagger UI to test encryption and decryption endpoints interactively

---

## 🛡️ Security Notes
- Always use HTTPS in production to protect data in transit.
- Store keys securely using Azure Key Vault, AWS Secrets Manager, or environment variables.
- Validate and sanitize all inputs to prevent injection attacks.
- Consider adding authentication and authorization to restrict access to the API.
