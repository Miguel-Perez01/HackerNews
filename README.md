# HackerNews Best Stories API

## Descripción

Este proyecto es una API RESTful que obtiene y devuelve los mejores `n` artículos de Hacker News ordenados por su puntuación, utilizando la [API pública de Hacker News](https://github.com/HackerNews/API).

La aplicación utiliza `ASP.NET Core`, emplea **caching en memoria** para mejorar el rendimiento y es capaz de manejar un gran número de solicitudes sin sobrecargar la API de Hacker News.

---

## Cómo ejecutar la aplicación

### Requisitos previos

1. .NET SDK 6.0 o superior instalado en tu sistema.
2. Un IDE compatible como Visual Studio, JetBrains Rider o Visual Studio Code.

---

### Pasos para ejecutar

1. Clona este repositorio:
   ```bash
   git clone https://github.com/Miguel-Perez01/HackerNews.git
   cd hackernews-beststories
