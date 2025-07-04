# MediaService

## Overview

**MediaService** is a simple and extensible media management service built with **.NET 9**.  
It provides an API for uploading and serving media files using [MinIO](https://min.io/) as an object storage backend and integrates with **RabbitMQ** to publish events when a new file is uploaded.

---

## ✨ Features

- Upload media files to MinIO
- Serve media files directly from MinIO
- Publish events to RabbitMQ when a file is uploaded
- Clean and testable architecture using abstraction layers
- Built with the latest .NET 9 framework

---

## 💻 Tech Stack

- **.NET 9**
- **MinIO** (object storage)
- **RabbitMQ** (event bus)
- **Entity Framework Core** (for in-memory token storage)

---

## 🚀 Getting Started

### Prerequisites

- .NET 9 SDK
- Docker
- Running MinIO instance
- Running RabbitMQ instance

### Setup

1. Clone the repository:

   ```bash
   git clone https://github.com/hvaezapp/MediaService.git
