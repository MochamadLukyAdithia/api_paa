  import 'package:another_flushbar/flushbar.dart';
  import 'package:flutter/material.dart';
  import 'package:mobile/tugas_mandiri/tugas_mandiri_controller.dart';
  import 'package:mobile/tugas_mandiri/tugas_mandiri_detail_view.dart';
  import 'package:mobile/tugas_mandiri/tugas_mandiri_model.dart';

  class TugasMandiriView extends StatelessWidget {
    const TugasMandiriView({super.key});
    void showCustomSnackBar(BuildContext context, String message) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(message),
          behavior: SnackBarBehavior
              .floating, // biar mengambang, kelihatan lebih modern
          backgroundColor: Colors.red, // custom warna
        ),
      );
    }

    @override
    Widget build(BuildContext context) {
      return Scaffold(
        appBar: AppBar(
          centerTitle: true,
          title: const Text(
            "Tugas Mandiri View Get Api",
            style: TextStyle(
              color: Colors.black,
              fontWeight: FontWeight.w900,
              fontSize: 24,
            ),
          ),
        ),
        body: FutureBuilder(
            future: CoffeController.fetchAllCoffe().catchError((value) {
              Flushbar(
                message: "Failed to load data",
                duration: Duration(seconds: 3),
                backgroundColor: Colors.red,
                borderRadius: BorderRadius.circular(8),
                margin: EdgeInsets.all(8),
              )..show(context);
              return <Coffe>[];
            }),
            builder: (context, snapshot) {
              if (snapshot.connectionState == ConnectionState.waiting) {
                return const Center(
                  child: CircularProgressIndicator(),
                );
              }
              if (snapshot.hasError) {
                return Center(
                  child: Text("Error: ${snapshot.error}"),
                );
              }
              if (snapshot.connectionState == ConnectionState.done &&
                  snapshot.hasData) {
                final coffe = snapshot.data!;
                return ListView.builder(
                  scrollDirection: Axis.vertical,
                  itemCount: coffe.length,
                  itemBuilder: (context, index) {
                    return ListTile(
                      leading: Image.network(coffe[index].image ?? "No Image"),
                      title: Text(coffe[index].title ?? "No Title"),
                      subtitle:
                          Text(coffe[index].description ?? "No Description"),
                      onTap: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(
                            builder: (context) => CoffeDetailView(
                              snapshot: coffe[index],
                            ),
                          ),
                        );
                      },
                    );
                  },
                );
              }
              return const Center(child: Text("No Data"));
            }),
      );
    }
  }
