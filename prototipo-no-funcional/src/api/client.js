const BASE_URL = import.meta.env.VITE_API_URL || "http://localhost:5172/api";

class ApiError extends Error {
  constructor(message, status, body) {
    super(message);
    this.status = status;
    this.body = body;
  }
}

async function request(path, options = {}) {
  const res = await fetch(BASE_URL + path, {
    headers: { "Content-Type": "application/json" },
    ...options,
  });

  if (res.status === 204) return null;

  const text = await res.text();
  let data = null;
  if (text) {
    data = JSON.parse(text);
  }

  if (!res.ok) {
    let message = "Error " + res.status + " al comunicarse con la API";
    if (data && data.title) message = data.title;
    if (data && data.message) message = data.message;
    throw new ApiError(message, res.status, data);
  }

  return data;
}

export function createResourceClient(resource) {
  return {
    list: () => request("/" + resource),
    get: (id) => request("/" + resource + "/" + id),
    create: (payload) =>
      request("/" + resource, {
        method: "POST",
        body: JSON.stringify(payload),
      }),
    update: (id, payload) =>
      request("/" + resource + "/" + id, {
        method: "PUT",
        body: JSON.stringify(payload),
      }),
    remove: (id) =>
      request("/" + resource + "/" + id, {
        method: "DELETE",
      }),
  };
}

export { ApiError, BASE_URL, request };
