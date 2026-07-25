# Combat Soldier

Combat Soldier is a 3D Real-Time Strategy (RTS) prototype developed with Unity. The project focuses on building a scalable, maintainable, and extensible gameplay architecture rather than creating a content-complete game.

It serves as a portfolio project demonstrating software architecture, SOLID principles, clean code practices, and engineering approaches. The primary goal was to design scalable and maintainable gameplay systems that remain easy to extend while keeping components loosely coupled and highly cohesive.

The project was built as an architectural showcase of modern Unity development practices, with a strong emphasis on modularity, flexibility, and long-term maintainability. Every major system was designed to minimize dependencies, isolate responsibilities, and allow new features to be integrated with minimal changes to the existing codebase.

---

# Architecture & Scalability

## Decentralized Gameplay Logic

Unit controllers intentionally contain very little business logic. Their primary responsibility is to compose gameplay components and coordinate state transitions, while complex calculations such as combat, targeting, and decision-making are delegated to specialized systems. This separation of responsibilities keeps controllers lightweight and significantly improves maintainability.

## Group-Based Processing

Instead of allowing every unit to perform expensive calculations independently every frame, many gameplay operations are centralized within global managers operating at a higher level of abstraction. This approach reduces duplicated work, improves scalability, and makes the architecture more suitable for large numbers of active entities.

## Event-Driven Architecture

The project follows an event-driven architecture where independent systems communicate through events instead of direct object references. This greatly reduces coupling between gameplay modules and allows new systems to be introduced without modifying existing implementations.

## Data-Driven Game Design

Core gameplay configuration is completely separated from source code using ScriptableObjects. Unit statistics, combat parameters, movement settings, and balancing values can all be modified directly by designers without requiring code changes or recompilation.

---

# Design Patterns & Technologies

## Dependency Injection

Dependency Injection using **Zenject** serves as the foundation of the project's architecture. Services, managers, repositories, factories, and the event bus are injected through DI containers, eliminating hard dependencies and making systems easier to maintain, extend, and unit test.

## Finite State Machine

Unit behavior is implemented using a Finite State Machine. Each state is encapsulated within its own class and follows a clearly defined lifecycle (`Start`, `Stop`) with event-driven control model. States manage their own subscriptions and internal logic, preventing large monolithic controller classes and keeping behavior isolated.

## Abstract Factory & Factory Method

Object creation is delegated to dedicated factories instead of being performed directly throughout the codebase. The Factory Method pattern encapsulates the creation of individual gameplay objects, while Abstract Factory groups related creation logic behind common interfaces. This approach reduces coupling, centralizes instantiation logic, and makes it straightforward to introduce new object families or replace existing implementations without affecting client code.

## Strategy Pattern

Animation handling, target searching, and distance evaluation are abstracted behind interchangeable interfaces using the Strategy pattern. Different algorithms can be swapped at runtime without modifying existing code, making experimentation and future extensions straightforward.

## Observer & Event Bus

Communication between loosely coupled systems — such as combat, UI, models, and gameplay flow—is implemented through a centralized Event Bus. Systems publish events without knowledge of their subscribers, allowing independent modules to react without introducing unnecessary dependencies.

## Object Pool

Projectile spawning is optimized through Object Pooling to eliminate unnecessary runtime allocations and reduce garbage collection spikes. Instead of repeatedly instantiating and destroying projectiles, previously allocated objects are efficiently recycled throughout gameplay.
