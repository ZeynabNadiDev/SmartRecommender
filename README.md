# 🧠 SmartRecommender

A modular recommendation system built with **Clean Architecture** and **AI-driven intent extraction** using `.NET 8`.

## 📘 Layers (5-Layer Clean Architecture)
- **API Layer** – Web API & endpoint routing  
- **AI Layer** – Intent extraction, NLP-based recommendations  
- **Application Layer** – Use cases, repository interfaces, DTOs  
- **Domain Layer** – Entities and business logic (pure models)  
- **Infrastructure Layer** – EF Core repositories, database context, and seed data  

## ⚙️ Features
- AI-driven _user intent parsing_  
- Bidirectional category normalization (Persian ↔ English)  
- Multi-criteria filtering (category, keyword, price, rating)  
- EF Core, DDD-compliant structure  

## 🧩 Testing Summary
All filters and seed data verified:
- Category + Keyword filters fixed  
- Price/quality range handling tested (200 OK)  
- Tests confirmed across 5 main user scenarios  

## 📄 Documentation
- [📘 English Technical Document (PDF)](./README_EN.pdf)
- [📙 مستند فنی فارسی (PDF)](./README_FA.pdf)

---

© 2025 | SmartRecommender by Zeynab Nadi
