(ns my-client.core
  (:require [org.httpkit.client :as http]
            [org.httpkit.timer :as timer])
  (:gen-class))


(comment
  (let [request (http/get "http://localhost:18080/cancellable1" {:as :text :timeout 2000})]
    (clojure.pprint/pprint @request))
  ;;---- 
  (let [request (http/get "http://localhost:18080/cancellable2" {:as :text :timeout 2000})]
    (clojure.pprint/pprint @request))
  ;;----
  )

(defn -main
  "I don't do a whole lot ... yet."
  [& args]
  (println "Hello, World!"))