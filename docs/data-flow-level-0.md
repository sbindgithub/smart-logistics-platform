# 📊 Data Flow Diagram (DFD) – Level 0  
## System Context Diagram

---

## 1. Purpose
This document describes the **high-level data flow** between external actors and the **Smart Logistics Platform**.
It focuses on **what data enters and leaves the system**, without internal technical details.

---

## 2. System Under Consideration
**Smart Logistics Platform**

The platform manages:
- Orders
- Inventory availability
- Shipments
- Notifications
- Operational visibility

---

## 3. External Entities (Actors)

### 3.1 Customer
- Places orders
- Views order and shipment status

### 3.2 Operations User
- Manages orders and shipments
- Monitors inventory and logistics status

### 3.3 External Partner System
- Sends shipment or logistics updates
- Receives order and shipment information

---

## 4. High-Level Data Flows

### 4.1 Customer ↔ Smart Logistics Platform
**Incoming Data**
- Order request
- Order modification request

**Outgoing Data**
- Order confirmation
- Order status
- Shipment tracking information

---

### 4.2 Operations User ↔ Smart Logistics Platform
**Incoming Data**
- Inventory updates
- Shipment status updates
- Operational commands

**Outgoing Data**
- Operational dashboards
- Alerts and notifications
- Reports

---

### 4.3 External Partner System ↔ Smart Logistics Platform
**Incoming Data**
- Shipment updates
- Logistics events

**Outgoing Data**
- Order details
- Shipment instructions
- Acknowledgements

---

## 5. DFD Level 0 – Conceptual View (Textual)

+-------------------+ Order / Status +---------------------------+
| Customer | <-------------------------> | |
+-------------------+ | |
| |
+-------------------+ Ops Commands / Reports | Smart Logistics Platform|
| Operations User | <-------------------------> | |
+-------------------+ | |
| |
+-------------------+ Shipment / Logistics Data | |
| External Partner | <-------------------------> | |
+-------------------+ +---------------------------+


---

## 6. Notes
- This diagram intentionally **hides internal services and databases**
- Internal decomposition is handled in **DFD Level 1**
- Technology choices are documented in **architecture.md**

---

## 7. Next Level
The next step is **DFD Level 1**, where the platform is decomposed into:
- Order Service
- Inventory Service
- Shipment Service
- Data Stores
