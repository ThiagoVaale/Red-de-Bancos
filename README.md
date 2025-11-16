
A continuación, te presento el diseño propuesto para las APIs externas, centrándonos en el *endpoint* de transferencia, los *payloads* (cuerpo de la petición) y las respuestas que simulan tus estrategias.

---

## 🏛️ Diseño de APIs Externas Ficticias (BadBank / WorseBank / WorstBank)

### 1. ⚙️ Convención de Datos Común

Asumimos que todos los bancos esperan un *payload* de transferencia similar. Esto es lo que tu `ExternalTransferRequestDto` (o una versión *JSON* del mismo) enviaría:

| Campo (JSON) | Tipo | Descripción |
| :--- | :--- | :--- |
| `transferId` | `string` (UUID) | ID único de la transferencia en GoodBank (trazabilidad). |
| `amount` | `decimal` | Monto de la transferencia. |
| `currency` | `string` (ej. "USD") | Código de divisa (ISO 4217). |
| `accountOriginRef` | `string` | Referencia de la cuenta de origen (opcional). |
| `accountDestination` | `string` | ID o CLABE de la cuenta de destino en el banco externo. |

---

### 2. 🏦 BadBank: La API Confiable (201 Created)

**Endpoint:** `/api/v1/payments/execute`
**Método:** `POST`

| Aspecto | Detalles |
| :--- | :--- |
| **Respuesta Éxito** | **HTTP 201 Created** |
| **Cuerpo de Respuesta** | `{ "externalReference": "BB-1A2B3C4D", "status": "PENDING" }` |
| **Simulación** | Siempre devuelve 201. Tu estrategia (`BadBankStrategy`) devuelve `IsSuccess: true`. |

---

### 3. 📉 WorseBank: La API Aleatoria (50% Éxito / 50% Error)

**Endpoint:** `/api/v2/transfers`
**Método:** `POST`

| Aspecto | Detalles |
| :--- | :--- |
| **Respuesta Éxito** | **HTTP 200 OK** |
| **Cuerpo de Respuesta** | `{ "referenceId": "WB-X1Y2Z3A4", "message": "Transaction processed." }` |
| **Respuesta Error (Temporal)** | **HTTP 503 Service Unavailable** |
| **Cuerpo de Error** | `{ "errorCode": "TEMP_DOWN", "detail": "Service is temporarily overloaded." }` |
| **Respuesta Error (Permanente)** | **HTTP 400 Bad Request** |
| **Cuerpo de Error** | `{ "errorCode": "INVALID_DATA", "detail": "Missing account destination field." }` |
| **Simulación** | Tu estrategia (`WorseBankStrategy`) simula un resultado aleatorio, y mapea 503 a `IsTransientError: true` y 400 a `IsTransientError: false`. |

---

### 4. 💣 WorstBank: La API Conflictiva (Suele Fallar)

**Endpoint:** `/legacy/transactions/process-payment`
**Método:** `POST`

| Aspecto | Detalles |
| :--- | :--- |
| **Respuesta Éxito** | **HTTP 202 Accepted** (Aceptado para procesamiento) |
| **Cuerpo de Respuesta** | `{ "processRef": "WSB-98765432" }` |
| **Respuesta Error (Timeout/Gateway)** | **HTTP 504 Gateway Timeout** |
| **Cuerpo de Error** | No hay respuesta (conexión perdida), forzando un **`TaskCanceledException`** en tu lado. |
| **Respuesta Error (Servidor Interno)** | **HTTP 500 Internal Server Error** |
| **Cuerpo de Error** | `{ "code": "INTERNAL", "message": "Unexpected error during execution." }` |
| **Simulación** | Tu estrategia (`WorstBankStrategy`) simula latencia alta y mapea 504/500 a `IsTransientError: true`, forzando la lógica de reintento. |

Este diseño te da una estructura clara para que tu código simule los diferentes escenarios de éxito y fallo que se esperan en una red bancaria real, basándose en códigos de estado HTTP estándar.
