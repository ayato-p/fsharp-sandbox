(ns app.core
  (:require [clj-http.client :as http]
            [clojure.java.io :as io]
            [cheshire.core :as json]))



(comment
  ;; https://github.com/dakrone/clj-http#incrementally-json-parsing
  (let [response (http/get "http://localhost:18080/stream" {:as :reader})]
    (with-open [r (:body response)]
      (doall (map println (json/parse-stream r true)))))

  (let [response (http/get "http://localhost:18080/large" {:as :json})]
    (println (first (drop 10 (:body response)))))
  ;;
  )